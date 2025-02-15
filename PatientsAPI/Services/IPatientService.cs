﻿using PatientsAPI.Models;

namespace PatientsAPI.Services
{
    public interface IPatientService
    {
        public Task<PatientModel?> GetById(int id);

        public Task<IEnumerable<PatientModel>> GetAll();

        public Task<bool> Exists(int id);

        public Task Add(PatientModelAdd modelAdd);

        public Task Update(int id, PatientModelUpdate modelUpdate);

        public Task Delete(int id);
    }
}
