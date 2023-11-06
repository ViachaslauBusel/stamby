using AI;
using GameField;
using Pathfinder;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField]
        private PlayerController m_playerControllerObj;
        [SerializeField]
        private GameGrid m_myGridObj;
        [SerializeField]
        private GameOver m_gameOverObj;


        public override void InstallBindings()
        {
            Container.Bind<PlayerController>().FromInstance(m_playerControllerObj).AsSingle();
            Container.Bind<GameGrid>().FromInstance(m_myGridObj).AsSingle();
            Container.Bind<GameOver>().FromInstance(m_gameOverObj).AsSingle();

            Container.Bind<AiStateFactory>().FromNew().AsSingle().NonLazy();
        }
    }
}