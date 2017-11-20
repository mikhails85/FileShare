namespace IPFS.Results
{
    public class Result<T>:VoidResult
    {
        public override bool Success => this.Value != null && base.Success;
        
        public T Value {get; private set;}

        public void SetValue(T value) => this.Value = value;
    }
}