public interface IUpdateObserver
{
    void ObservedUpdate();


    void OnEnable()
    {
        UpdatePublisher.RegisterUpdateObserver(this);
    }


    void OnDisable()
    {
        UpdatePublisher.UnregisterUpdateObserver(this);
    }
}