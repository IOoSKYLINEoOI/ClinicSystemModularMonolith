using Patients.Core.Models;

namespace Patients.Core.Interfaces.Repository;

public interface IQuestionnaireRepository
{
    Task Add(Questionnaire questionnaire);
    Task Update(Questionnaire questionnaire);
    Task Delete(Guid id);
    Task<Questionnaire?> GetById(Guid id);
    Task<List<Questionnaire>> GetByAllPatientId(Guid patientId);
}