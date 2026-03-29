using System.Collections.ObjectModel;
using Contacts.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Models;

/// <summary>
/// In-memory repository.  Acts as the single source of truth for the
/// ObservableCollection that the ViewModel binds to.
/// </summary>
public class ContactRepository
{
    private int _nextId = 1;
    private readonly AppDbContext _db;

    public List<Group> GetGroups() => _db.Groups.ToList();
    public ObservableCollection<Contact> Contacts { get; } = new();

    public ContactRepository()
    {
        _db = new AppDbContext();
        _db.Database.Migrate();

        foreach (var c in _db.Contacts.Include(c => c.Group))
            Contacts.Add(c);

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
        existing.GroupId   = updated.GroupId;
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

        var family = new Group { Name = "Family" };
        var friend = new Group { Name = "Friend" };
        var work = new Group { Name = "Work" };

        _db.Groups.AddRange(family, friend, work);
        _db.SaveChanges();

        var seed = new[]
        {
            new Contact { FirstName="Anna",    LastName="Kowalska",  Email="anna.kowalska@example.com",  Phone="600-111-222", City="Warsaw",   Age=28, GroupId=work.Id    },
            new Contact { FirstName="Piotr",   LastName="Nowak",     Email="piotr.nowak@example.com",    Phone="601-222-333", City="Kraków",   Age=35, GroupId=friend.Id },
            new Contact { FirstName="Marta",   LastName="Wiśniewska",Email="marta.w@example.com",        Phone="602-333-444", City="Gdańsk",   Age=22, GroupId=family.Id },
            new Contact { FirstName="Tomasz",  LastName="Wójcik",    Email="tomasz.w@example.com",       Phone="603-444-555", City="Warsaw",   Age=41, GroupId=work.Id   },
            new Contact { FirstName="Karolina",LastName="Kowalczyk", Email="karo.k@example.com",         Phone="604-555-666", City="Wrocław",  Age=30, GroupId=friend.Id },
            new Contact { FirstName="Marek",   LastName="Kaminski",  Email="marek.k@example.com",        Phone="605-666-777", City="Poznań",   Age=50, GroupId=family.Id },
            new Contact { FirstName="Agata",   LastName="Lewandowska",Email="agata.l@example.com",       Phone="606-777-888", City="Łódź",     Age=19, GroupId=friend.Id },
            new Contact { FirstName="Krzysztof",LastName="Zielinski",Email="krzys.z@example.com",        Phone="607-888-999", City="Kraków",   Age=38, GroupId=work.Id  },
            new Contact { FirstName="Ewa",     LastName="Szymańska", Email="ewa.s@example.com",          Phone="608-999-000", City="Warsaw",   Age=45, GroupId=family.Id },
            new Contact { FirstName="Rafał",   LastName="Woźniak",   Email="rafal.w@example.com",        Phone="609-000-111", City="Gdańsk",   Age=27, GroupId=work.Id   },
        };

        foreach (var c in seed)
            Add(c);
    }
}
