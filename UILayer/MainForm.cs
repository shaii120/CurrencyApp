namespace UILayer;

using System.Configuration;
using System.Drawing;
using BusinessLayer;
using DataLayer;

public partial class MainForm : Form
{
    private readonly SimulationService _service;
    private readonly BindingSource _bindingSource = new();
    private readonly Dictionary<int, decimal> _previousValues = new();

    public MainForm()
    {
        InitializeComponent();
        string connectionString = ConfigurationManager
            .ConnectionStrings["CurrencyDb"]
            .ConnectionString;
        var repo = new CurrencyPairRepository(connectionString);
        _service = new SimulationService(repo);

        InitiallizeWindow();

        _bindingSource.DataSource = _service.GetPairs();
        dataGridView1.DataSource = _bindingSource;
        dataGridView1.CellFormatting += DataGridView1_CellFormatting;

        InitiallizeGrid1();

        _service.PairsUpdated += RefreshGrid;
    }

    private void InitiallizeWindow()
    {
        Text = "Currency Rates Simulator";
        Width = 900;
        Height = 400;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
    }
    private void InitiallizeGrid1()
    {
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView1.Columns["Id"]?.Visible = false;
        dataGridView1.Columns["BaseCode"]?.HeaderText = "Base Currency";
        dataGridView1.Columns["QuoteCode"]?.HeaderText = "Quote Currency";
        dataGridView1.Columns["CurrentValue"]?.HeaderText = "Current Rate";
        dataGridView1.Columns["MinValue"]?.HeaderText = "Minimum";
        dataGridView1.Columns["MaxValue"]?.HeaderText = "Maximum";
        dataGridView1.Columns["CurrentValue"]?.DefaultCellStyle.Format = "0.0000";
        dataGridView1.Columns["MinValue"]?.DefaultCellStyle.Format = "0.0000";
        dataGridView1.Columns["MaxValue"]?.DefaultCellStyle.Format = "0.0000";
        dataGridView1.ReadOnly = true;
        dataGridView1.AllowUserToAddRows = false;
    }

    private void RefreshGrid()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(RefreshGrid));
            return;
        }

        _bindingSource.ResetBindings(false);
        dataGridView1.Refresh();

        foreach (CurrencyPair pair in _bindingSource.List)
        {
            _previousValues[pair.Id] = pair.CurrentValue;
        }
    }

    private void DataGridView1_CellFormatting(
    object? sender,
    DataGridViewCellFormattingEventArgs e)
    {
        var grid = (DataGridView)sender!;
        if (grid.Columns[e.ColumnIndex].Name != "CurrentValue")
        {
            return;
        }

        var pair = grid.Rows[e.RowIndex].DataBoundItem as CurrencyPair;

        if (pair == null)
        {
            return;
        }
        _previousValues.TryGetValue(pair.Id, out var previous);

        if (pair.CurrentValue > previous)
        {
            e.CellStyle.ForeColor = Color.Green;
        }
        else if (pair.CurrentValue < previous)
        {
            e.CellStyle.ForeColor = Color.Red;
        }
        else
        {
            e.CellStyle.ForeColor = Color.Black;
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        Task.Run(() => _service.Start());
    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }
}
