using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contacts.Models;

namespace Contacts.ViewModels;

public partial class MainViewModel : ObservableObject
{
    // ─── Repository ──────────────────────────────────────────────────────────
    private readonly ContactRepository _repo = new();

    // The master list we always LINQ against
    public ObservableCollection<Contact> AllContacts => _repo.Contacts;

    // The list shown in the DataGrid (filtered / sorted / projected view)
    [ObservableProperty]
    private ObservableCollection<Contact> _displayedContacts = new();

    // ─── Selection ───────────────────────────────────────────────────────────
    [ObservableProperty]
    private Contact? _selectedContact;

    // ─── Filter ──────────────────────────────────────────────────────────────
    public static IReadOnlyList<string> FilterOptions { get; } = new[]
    {
        "No Filter",
        "By City (Warsaw)"
     
    };

    [ObservableProperty]
    private string _selectedFilter = "No Filter";

    // ─── Sort ────────────────────────────────────────────────────────────────
    public static IReadOnlyList<string> SortOptions { get; } = new[]
    {
        "Last Name (A→Z)",
        "Last Name (Z→A)"
    };

    [ObservableProperty]
    private string _selectedSort = "Last Name (A→Z)";

    // ─── Projection ──────────────────────────────────────────────────────────
    public static IReadOnlyList<string> ProjectionOptions { get; } = new[]
    {
        "All Columns",
        "Full Name + Email"
    };

    [ObservableProperty]
    private string _selectedProjection = "All Columns";

    // Column visibility (driven by projection)
    [ObservableProperty] private bool _showEmail  = true;
    [ObservableProperty] private bool _showPhone  = true;
    [ObservableProperty] private bool _showCity   = true;
    [ObservableProperty] private bool _showAge    = true;
    [ObservableProperty] private bool _showGroup  = true;

    // ─── Quantifier ──────────────────────────────────────────────────────────
    public static IReadOnlyList<string> QuantifierOptions { get; } = new[]
    {
        "Any contact in Warsaw?"
    };

    [ObservableProperty]
    private string _selectedQuantifier = "Any contact in Warsaw?";

    [ObservableProperty]
    private string _quantifierResult = string.Empty;

    // ─── Aggregation ─────────────────────────────────────────────────────────
    public static IReadOnlyList<string> AggregationOptions { get; } = new[]
    {
        "Average Age"
    };

    [ObservableProperty]
    private string _selectedAggregation = "Average Age";

    [ObservableProperty]
    private string _aggregationResult = string.Empty;

    // ─── Result panel ────────────────────────────────────────────────────────
    [ObservableProperty]
    private string _linqResultText = string.Empty;

    // ─── Constructor ─────────────────────────────────────────────────────────
    public MainViewModel()
    {
        ApplyLinq();
    }

    // ─── LINQ Pipeline ───────────────────────────────────────────────────────

    [RelayCommand]
    public void ApplyLinq()
    {
        IEnumerable<Contact> query = AllContacts;

        // 1. FILTER ──────────────────────────────────────────────────────────
        query = SelectedFilter switch
        {
            "By City (Warsaw)" => query.Where(c => c.City == "Warsaw"),
            _                  => query   // "No Filter"
        };

        // 2. SORT ────────────────────────────────────────────────────────────
        query = SelectedSort switch
        {
            "Last Name (Z→A)"  => query.OrderByDescending(c => c.LastName),
            _                  => query.OrderBy(c => c.LastName)   // A→Z default
        };

        // 3. PROJECTION – apply column visibility flags ──────────────────────
        ApplyProjection();
        // (We keep full Contact objects in DisplayedContacts so DataGrid
        //  columns can be hidden/shown; the projection is expressed via
        //  column Visibility bindings rather than anonymous types.)

        DisplayedContacts = new ObservableCollection<Contact>(query);

        // 4. QUANTIFIER ──────────────────────────────────────────────────────
        bool quantResult = SelectedQuantifier switch
        {
            "All contacts have email?" => AllContacts.All(c => !string.IsNullOrWhiteSpace(c.Email)),
            _                         => AllContacts.Any(c => c.City == "Warsaw")
        };
        QuantifierResult = $"{SelectedQuantifier}  →  {quantResult}";

        // 5. AGGREGATION ─────────────────────────────────────────────────────
        if (AllContacts.Count == 0)
        {
            AggregationResult = "No contacts.";
        }
        else
        {
            AggregationResult = SelectedAggregation switch
            {
                "Oldest Contact"  => $"Oldest: {AllContacts.Max(c => c.Age)} years",
                _                 => $"Avg Age: {AllContacts.Average(c => c.Age):F1} years"
            };
        }

        // Summary panel
        LinqResultText = BuildSummary();
    }

    private void ApplyProjection()
    {
        switch (SelectedProjection)
        {
            case "Full Name + Email":
                ShowEmail = true;  ShowPhone = false;
                ShowCity  = false; ShowAge   = false; ShowGroup = false;
                break;
            default: // All Columns
                ShowEmail = true; ShowPhone = true;
                ShowCity  = true; ShowAge   = true; ShowGroup = true;
                break;
        }
    }

    private string BuildSummary()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Showing {DisplayedContacts.Count} of {AllContacts.Count} contact(s).");
        sb.AppendLine($"Filter   : {SelectedFilter}");
        sb.AppendLine($"Sort     : {SelectedSort}");
        sb.AppendLine($"Projection: {SelectedProjection}");
        sb.AppendLine($"Quantifier: {QuantifierResult}");
        sb.AppendLine($"Aggregation: {AggregationResult}");
        return sb.ToString();
    }

    // ─── CRUD Commands ───────────────────────────────────────────────────────

    // Signals for the View to open dialogs
    public event Action<Contact?>? OpenAddDialog;
    public event Action<Contact>?  OpenEditDialog;

    [RelayCommand]
    private void AddContact() => OpenAddDialog?.Invoke(null);

    [RelayCommand]
    private void EditContact()
    {
        if (SelectedContact is null) return;
        OpenEditDialog?.Invoke(SelectedContact);
    }

    [RelayCommand]
    private void DeleteContact()
    {
        if (SelectedContact is null) return;
        _repo.Delete(SelectedContact);
        ApplyLinq();
    }

    // Called by the dialog on confirm
    public void SaveNewContact(Contact c)
    {
        _repo.Add(c);
        ApplyLinq();
    }

    public void SaveEditedContact(Contact original, Contact updated)
    {
        _repo.Update(original, updated);
        ApplyLinq();
    }
}
