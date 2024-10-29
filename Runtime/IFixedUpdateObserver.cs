public interface IFixedUpdateObserver
{
    void ObservedFixedUpdate();


    void OnEnable()
    {
        UpdatePublisher.RegisterFixedUpdateObserver(this);
    }


    void OnDisable()
    {
        UpdatePublisher.UnregisterFixedUpdateObserver(this);
    }
}