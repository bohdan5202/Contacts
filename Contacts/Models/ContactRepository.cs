using System.Collections.ObjectModel;
using Contacts.Models;

namespace Contacts.Models;

/// <summary>
/// In-memory repository.  Acts as the single source of truth for the
/// ObservableCollection that the ViewModel binds to.
/// </summary>
public class ContactRepository
{
    private int _nextId = 1;
    private readonly AppDbContext _db;

    public ObservableCollection<Contact> Contacts { get; } = new();

    public ContactRepository()
    {
        _db = new AppDbContext();
        _db.Database.EnsureCreated();

        foreach (var c in _db.Contacts)
        {
            Contacts.Add(c);
        }
        if (!Contacts.Any())
            Seed();
    }

    // ─── CRUD ────────────────────────────────────────────────────────────────

    public void Add(Contact contact)
    {
        _db.Contacts.Add(contact);
        _db.SaveChanges();
        Contacts.Add(contact);
    }

    public void Update(Contact existing, Contact updated)
    {
        existing.FirstName = updated.FirstName;
        existing.LastName  = updated.LastName;
        existing.Email     = updated.Email;
        existing.Phone     = updated.Phone;
        existing.City      = updated.City;
        existing.Age       = updated.Age;
        existing.Group     = updated.Group;
        _db.SaveChanges();
    }

    public void Delete(Contact contact)
    {
        _db.Contacts.Remove(contact);
        _db.SaveChanges(); 
        Contacts.Remove(contact);
    }
    // ─── Seed data ───────────────────────────────────────────────────────────

    private void Seed()
    {
        var seed = new[]
        {
            new Contact { FirstName="Anna",    LastName="Kowalska",  Email="anna.kowalska@example.com",  Phone="600-111-222", City="Warsaw",   Age=28, Group="Work"   },
            new Contact { FirstName="Piotr",   LastName="Nowak",     Email="piotr.nowak@example.com",    Phone="601-222-333", City="Kraków",   Age=35, Group="Friend" },
            new Contact { FirstName="Marta",   LastName="Wiśniewska",Email="marta.w@example.com",        Phone="602-333-444", City="Gdańsk",   Age=22, Group="Family" },
            new Contact { FirstName="Tomasz",  LastName="Wójcik",    Email="tomasz.w@example.com",       Phone="603-444-555", City="Warsaw",   Age=41, Group="Work"   },
            new Contact { FirstName="Karolina",LastName="Kowalczyk", Email="karo.k@example.com",         Phone="604-555-666", City="Wrocław",  Age=30, Group="Friend" },
            new Contact { FirstName="Marek",   LastName="Kaminski",  Email="marek.k@example.com",        Phone="605-666-777", City="Poznań",   Age=50, Group="Family" },
            new Contact { FirstName="Agata",   LastName="Lewandowska",Email="agata.l@example.com",       Phone="606-777-888", City="Łódź",     Age=19, Group="Friend" },
            new Contact { FirstName="Krzysztof",LastName="Zielinski",Email="krzys.z@example.com",        Phone="607-888-999", City="Kraków",   Age=38, Group="Work"   },
            new Contact { FirstName="Ewa",     LastName="Szymańska", Email="ewa.s@example.com",          Phone="608-999-000", City="Warsaw",   Age=45, Group="Family" },
            new Contact { FirstName="Rafał",   LastName="Woźniak",   Email="rafal.w@example.com",        Phone="609-000-111", City="Gdańsk",   Age=27, Group="Work"   },
        };

        foreach (var c in seed)
            Add(c);
    }
}
