using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//SetSpawnState controla o estado do loop spawn, assim facilitando a ativação ou desativação do mesmo
public interface ISpawnObjects
{
    public void SetSpawnState(bool spawnState);
    IEnumerator SpawnLoop();
}
