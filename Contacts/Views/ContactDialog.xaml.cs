using System.Windows;
using System.Windows.Controls;
using Contacts.Models;

namespace Contacts.Views;

public partial class ContactDialog : Window
{
    public Contact Result { get; private set; } = new();

    public ContactDialog(Contact? existing = null)
    {
        InitializeComponent();

        if (existing is not null)
        {
            TxtFirstName.Text = existing.FirstName;
            TxtLastName.Text  = existing.LastName;
            TxtEmail.Text     = existing.Email;
            TxtPhone.Text     = existing.Phone;
            TxtCity.Text      = existing.City;
            TxtAge.Text       = existing.Age.ToString();

            // Select matching group item
            foreach (ComboBoxItem item in CmbGroup.Items)
                if (item.Content?.ToString() == existing.Group)
                {
                    CmbGroup.SelectedItem = item;
                    break;
                }

            Title = "Edit Contact";
        }
        else
        {
            CmbGroup.SelectedIndex = 0;
            Title = "Add Contact";
        }
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
            string.IsNullOrWhiteSpace(TxtLastName.Text))
        {
            MessageBox.Show("First Name and Last Name are required.",
                            "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(TxtAge.Text, out int age) || age < 0 || age > 150)
        {
            MessageBox.Show("Please enter a valid age (0–150).",
                            "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Result = new Contact
        {
            FirstName = TxtFirstName.Text.Trim(),
            LastName  = TxtLastName.Text.Trim(),
            Email     = TxtEmail.Text.Trim(),
            Phone     = TxtPhone.Text.Trim(),
            City      = TxtCity.Text.Trim(),
            Age       = age,
            Group     = (CmbGroup.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Friend"
        };

        DialogResult = true;
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
