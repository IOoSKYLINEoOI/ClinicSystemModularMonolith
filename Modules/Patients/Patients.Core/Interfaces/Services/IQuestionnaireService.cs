using CSharpFunctionalExtensions;
using Patients.Core.Models;

namespace Patients.Core.Interfaces.Services;

public interface IQuestionnaireService
{
    Task<Result> AddQuestionnaire(
        string? allergies,
        string? currentMedications,
        bool? isSmoker,
        bool? isAlcoholConsumer,
        double? heightCm,
        double? weightKg,
        Guid patientId);

    Task<Result> UpdateQuestionnaire(
        Guid id,
        string? allergies,
        string? currentMedications,
        bool? isSmoker,
        bool? isAlcoholConsumer,
        double? heightCm,
        double? weightKg);

    Task<Result<Questionnaire>> GetQuestionnaireById(Guid id);
    Task<Result<List<Questionnaire>>> GetAllQuestionnaireByPatientId(Guid patientId);
}