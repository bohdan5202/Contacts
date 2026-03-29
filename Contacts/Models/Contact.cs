using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Contacts.Models;

public partial class Contact : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _phone = string.Empty;

    [ObservableProperty]
    private string _city = string.Empty;

    [ObservableProperty]
    private int _age;

    public int GroupId { get; set; }
    public Group? Group { get; set; }
    public string FullName => $"{FirstName} {LastName}";

    public Contact Clone() => new Contact
    {
        Id        = Id,
        FirstName = FirstName,
        LastName  = LastName,
        Email     = Email,
        Phone     = Phone,
        City      = City,
        Age       = Age,
        GroupId = GroupId
    };
}
