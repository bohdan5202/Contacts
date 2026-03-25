using System.Windows;
using Contacts.ViewModels;
using Contacts.Views;

namespace Contacts;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;

    public MainWindow()
    {
        InitializeComponent();

        _vm = new MainViewModel();
        DataContext = _vm;

        // Wire dialog events
        _vm.OpenAddDialog  += OnOpenAddDialog;
        _vm.OpenEditDialog += OnOpenEditDialog;
    }

    private void OnOpenAddDialog(Models.Contact? _)
    {
        var dlg = new ContactDialog { Owner = this };
        if (dlg.ShowDialog() == true)
            _vm.SaveNewContact(dlg.Result);
    }

    private void OnOpenEditDialog(Models.Contact contact)
    {
        var dlg = new ContactDialog(contact) { Owner = this };
        if (dlg.ShowDialog() == true)
            _vm.SaveEditedContact(contact, dlg.Result);
    }
}
