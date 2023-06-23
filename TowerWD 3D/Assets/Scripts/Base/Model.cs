using CanasSource;

namespace Models
{
    public abstract class Model
    {
        public string Id { get; }

        protected Model()
        {
            Id = Singleton<InGameController>.Instance.SetIdForEnemy();
        }
    
        protected void DataChange(string nameOfValue)
        {
            Singleton<Observer>.Instance.InvokeDataChange(new EventBase(Id, nameOfValue, GetType().GetProperty(nameOfValue)?.GetValue(this)));
        }
    }
}
