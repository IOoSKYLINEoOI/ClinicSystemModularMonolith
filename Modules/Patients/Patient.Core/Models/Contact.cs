using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace Patient.Core.Models;

public class Contact
{
    private const int MaxLengthContactName = 60;
    private const int MaxLengthRelationship = 20;
    
    private Contact(
        Guid id, 
        Guid patientId, 
        string contactName, 
        string relationship, 
        string phone, 
        string? email)
    {
        Id = id;
        PatientId = patientId;
        ContactName = contactName;
        Relationship = relationship;
        Phone = phone;
        Email = email;
    }
    
    public Guid Id { get; }
    public Guid PatientId { get; }
    public string ContactName { get; }   
    public string Relationship { get; }  
    public string Phone { get; }
    public string? Email { get; }

    public static Result<Contact> Create(
        Guid id,
        Guid patientId,
        string contactName,
        string relationship,
        string phone,
        string? email)
    {
        if (patientId == Guid.Empty)
            return Result.Failure<Contact>("PatientId не может быть пустым GUID.");
        
        if (contactName.Length > MaxLengthContactName)
        {
            return Result.Failure<Contact>($"'{nameof(contactName)}' more than {MaxLengthContactName} characters.");
        }
        
        if (relationship.Length > MaxLengthRelationship)
        {
            return Result.Failure<Contact>($"'{nameof(relationship)}' more than {MaxLengthRelationship} characters.");
        }
        
        if (!Regex.IsMatch(phone, @"^\+?[0-9]{10,15}$"))
            return Result.Failure<Contact>("Неверный формат номера");
        
        if (email != null && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return Result.Failure<Contact>("Неверный формат электронной почты");
        
        var contact = new Contact(
            id,
            patientId,
            contactName,
            relationship,
            phone,
            email);
        
        return Result.Success(contact);
    }

    public static Contact FromPersistence(
        Guid id,
        Guid patientId,
        string contactName,
        string relationship,
        string phone,
        string? email)
    {
        return new Contact(
            id,
            patientId,
            contactName,
            relationship,
            phone,
            email);
    }
}