using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCloud.BusinessLogicLayer
{
    public abstract class BaseLogicSystem<TPoco>            
    {
        protected IDataRepository<TPoco> _repository;

        public string Code { get; private set; }

        public BaseLogicSystem(IDataRepository<TPoco> repository)
        {
            _repository = repository;
        }

        protected virtual void Verify(TPoco[] pocos)
        {
            return;
        }

        public virtual TPoco Get(string id)     
        {
            return _repository.GetSingle(c => Code== id);            
        }

        public virtual List<TPoco> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public virtual void Add(TPoco[] pocos)
        {
            foreach (TPoco poco in pocos)
            {
                //if (poco.Code == Guid.Empty)
                //{
                //    //poco.Id = Guid.NewGuid();
                //    poco.Code = Guid.NewGuid();
                //}
            }

            _repository.Add(pocos);
        }

        public virtual void Update(TPoco[] pocos)
        {
            _repository.Update(pocos);
        }

        public void Delete(TPoco[] pocos)
        {
            _repository.Remove(pocos);
        }
    }
}
