using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISetGameState
{
    public void SetGameState(GameState gameState);
    public void MainMenu();
    public void StartMatch();
    public void EndGame();
}
