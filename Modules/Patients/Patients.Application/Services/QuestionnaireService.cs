using CSharpFunctionalExtensions;
using Patients.Core.Interfaces.Repository;
using Patients.Core.Interfaces.Services;
using Patients.Core.Models;

namespace Patients.Application.Services;

public class QuestionnaireService : IQuestionnaireService
{
    private readonly IQuestionnaireRepository _questionnaireRepository;

    public QuestionnaireService(IQuestionnaireRepository questionnaireRepository)
    {
        _questionnaireRepository = questionnaireRepository;
    }

    public async Task<Result> AddQuestionnaire(
        string? allergies,
        string? currentMedications,
        bool? isSmoker,
        bool? isAlcoholConsumer,
        double? heightCm,
        double? weightKg,
        Guid patientId)
    {
        var questionnaireResult = Questionnaire.Create(
            id: Guid.NewGuid(),
            allergies: allergies,
            currentMedications: currentMedications,
            isSmoker: isSmoker,
            isAlcoholConsumer: isAlcoholConsumer,
            heightCm: heightCm,
            weightKg: weightKg,
            patientId: patientId,
            createdAt: DateTime.Now,
            updatedAt: DateTime.Now);
        
        if(questionnaireResult.IsFailure)
            return Result.Failure(questionnaireResult.Error);
        
        await _questionnaireRepository.Add(questionnaireResult.Value);
        return Result.Success();
    }

    public async Task<Result> UpdateQuestionnaire(
        Guid id,
        string? allergies,
        string? currentMedications,
        bool? isSmoker,
        bool? isAlcoholConsumer,
        double? heightCm,
        double? weightKg)
    {
        var questionnaire = await _questionnaireRepository.GetById(id);
        if(questionnaire == null)
            return Result.Failure($"Questionnaire with ID {id} not found");
        
        var questionnaireResult = Questionnaire.Create(
            id: questionnaire.Id,
            allergies: allergies,
            currentMedications: currentMedications,
            isSmoker: isSmoker,
            isAlcoholConsumer: isAlcoholConsumer,
            heightCm: heightCm,
            weightKg: weightKg,
            patientId: questionnaire.PatientId,
            createdAt: questionnaire.CreatedAt,
            updatedAt: DateTime.Now);
        
        if(questionnaireResult.IsFailure)
            return Result.Failure(questionnaireResult.Error);
        
        await _questionnaireRepository.Update(questionnaireResult.Value);
        return Result.Success();
    }

    public async Task<Result<Questionnaire>> GetQuestionnaireById(Guid id)
    {
        var questionnaire = await _questionnaireRepository.GetById(id);
        if(questionnaire == null)
            return Result.Failure<Questionnaire>($"Questionnaire with ID {id} not found");
        
        return Result.Success(questionnaire);
    }

    public async Task<Result<List<Questionnaire>>> GetAllQuestionnaireByPatientId(Guid patientId)
    {
        return await _questionnaireRepository.GetByAllPatientId(patientId);
    }
}