
/// <summary>
/// SetVolume manipulado por terceiros
/// Load volume recupera as configurações salvas para atribuir no start
/// </summary>
public interface ISetAudioVolume
{
    public void SetVolume(float volume);

    public void LoadVolume();
}
