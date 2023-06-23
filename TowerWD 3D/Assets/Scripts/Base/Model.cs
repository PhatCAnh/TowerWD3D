using CanasSource;

namespace Models
{
    public abstract class Model
    {
        public string Id { get; protected set; }

        protected void DataChange(string nameOfValue)
        {
            Singleton<Observer>.Instance.InvokeDataChange(new DataEventChange(Id ,nameOfValue, GetType().GetProperty(nameOfValue)?.GetValue(this)));
        }
    }
}
