using Microsoft.EntityFrameworkCore;
using Patients.Core.Interfaces.Repository;
using Patients.Core.Models;
using Patients.DataAccess.Entities;

namespace Patients.DataAccess.Repositories;

public class QuestionnaireRepository : IQuestionnaireRepository
{
    private readonly PatientDbContext _context;

    public QuestionnaireRepository(PatientDbContext context)
    {
        _context = context;
    }

    public async Task Add(Questionnaire questionnaire)
    {
        var questionnaireEntity = MapToEntity(questionnaire);

        _context.Questionnaires.Add(questionnaireEntity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Questionnaire questionnaire)
    {
        var questionnaireEntity = await _context.Questionnaires.FirstOrDefaultAsync(c => c.Id == questionnaire.Id);
        if(questionnaireEntity == null)
            throw new InvalidOperationException("Questionnaire not found");

        questionnaireEntity.Allergies = questionnaire.Allergies;
        questionnaireEntity.CurrentMedications = questionnaire.CurrentMedications;
        questionnaireEntity.IsSmoker = questionnaire.IsSmoker;
        questionnaireEntity.IsAlcoholConsumer = questionnaire.IsAlcoholConsumer;
        questionnaireEntity.HeightCm = questionnaire.HeightCm;
        questionnaireEntity.WeightKg = questionnaire.WeightKg;
        questionnaireEntity.UpdatedAt = questionnaire.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var questionnaireEntity = await _context.Questionnaires.FirstOrDefaultAsync(c => c.Id == id);
        if(questionnaireEntity == null)
            throw new InvalidOperationException("Questionnaire not found");

        _context.Questionnaires.Remove(questionnaireEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Questionnaire?> GetById(Guid id)
    {
        var questionnaireEntity = await _context.Questionnaires.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

        return questionnaireEntity == null ? null : MapToDomain(questionnaireEntity);
    }

    public async Task<List<Questionnaire>> GetByAllPatientId(Guid patientId)
    {
        var questionnaireEntity = await _context.Questionnaires
            .AsNoTracking()
            .Where(c => c.PatientId == patientId)
            .ToListAsync();

        return questionnaireEntity.Select(MapToDomain).ToList();
    }

    private static QuestionnaireEntity MapToEntity(Questionnaire questionnaire) => new QuestionnaireEntity()
    {
        Id = questionnaire.Id,
        Allergies = questionnaire.Allergies,
        CurrentMedications = questionnaire.CurrentMedications,
        IsSmoker = questionnaire.IsSmoker,
        IsAlcoholConsumer = questionnaire.IsAlcoholConsumer,
        HeightCm = questionnaire.HeightCm,
        WeightKg = questionnaire.WeightKg,
        PatientId = questionnaire.PatientId,
        CreatedAt = questionnaire.CreatedAt,
        UpdatedAt = questionnaire.UpdatedAt
    };
    
    private static Questionnaire MapToDomain(QuestionnaireEntity questionnaireEntity) => Questionnaire.FromPersistance(
        questionnaireEntity.Id,
        questionnaireEntity.Allergies,
        questionnaireEntity.CurrentMedications,
        questionnaireEntity.IsSmoker,
        questionnaireEntity.IsAlcoholConsumer,
        questionnaireEntity.HeightCm,
        questionnaireEntity.WeightKg,
        questionnaireEntity.PatientId,
        questionnaireEntity.CreatedAt,
        questionnaireEntity.UpdatedAt);
}