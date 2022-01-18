namespace MasterCode {

    using System.Collections.Generic;
    using UnityEngine;

    abstract public class MasterCoordinator<T> : MonoBehaviour where T : UnityEngine.MonoBehaviour {

        #region Virtual Methods

        virtual protected void Setup() { }
        virtual protected void Destroy() { }

        #endregion


        #region MonoBehaviour

        private void Start() {

            this.Setup();
            this.Run();
        }

        private void OnDestroy() {

            this.Destroy();
        }

        private void Update() {

            this.Updates_BeforeState.Update();
            this.FSM.Update();
            this.Updates_AfterState.Update();
        }

        private void LateUpdate() {

            this.LateUpdates_BeforeState.Update();
            this.FSM.LateUpdate();
            this.LateUpdates_AfterState.Update();
        }

        private void FixedUpdate() {

            this.FixedUpdates_BeforeState.Update();
            this.FSM.FixedUpdate();
            this.FixedUpdates_AfterState.Update();
        }

        #endregion


        #region Internal Fields

        private T Instance { get { return this as T; } }
        protected delegate void UpdateMethod();
        private FiniteStateMachine<T> m_FSM;
        private UpdateController m_Updates_BeforeState;
        private UpdateController m_Updates_AfterState;
        private UpdateController m_LateUpdates_BeforeState;
        private UpdateController m_LateUpdates_AfterState;
        private UpdateController m_FixedUpdates_BeforeState;
        private UpdateController m_FixedUpdates_AfterState;

        #endregion


        #region Internal Properties

        private FiniteStateMachine<T> FSM => m_FSM = m_FSM ?? new FiniteStateMachine<T>(this.Instance);
        private UpdateController Updates_BeforeState => m_Updates_BeforeState = m_Updates_BeforeState ?? new UpdateController();
        private UpdateController Updates_AfterState => m_Updates_AfterState = m_Updates_AfterState ?? new UpdateController();
        private UpdateController LateUpdates_BeforeState => m_LateUpdates_BeforeState = m_LateUpdates_BeforeState ?? new UpdateController();
        private UpdateController LateUpdates_AfterState => m_LateUpdates_AfterState = m_LateUpdates_AfterState ?? new UpdateController();
        private UpdateController FixedUpdates_BeforeState => m_FixedUpdates_BeforeState = m_FixedUpdates_BeforeState ?? new UpdateController();
        private UpdateController FixedUpdates_AfterState => m_FixedUpdates_AfterState = m_FixedUpdates_AfterState ?? new UpdateController();

        #endregion


        #region Internal Methods

        private void Run() {

            this.FSM.ChangeState(0);
        }

        protected void Add_State(State<T> state) {

            this.FSM.Add(state);
        }

        protected void Add_Updates_BeforeState(UpdateMethod method) {

            this.Updates_BeforeState.Add(method);
        }

        protected void Add_Updates_AfterState(UpdateMethod method) {

            this.Updates_AfterState.Add(method);
        }

        protected void Add_LateUpdates_BeforeState(UpdateMethod method) {

            this.LateUpdates_BeforeState.Add(method);
        }

        protected void Add_LateUpdates_AfterState(UpdateMethod method) {

            this.LateUpdates_AfterState.Add(method);
        }

        protected void Add_FixedUpdates_BeforeState(UpdateMethod method) {

            this.FixedUpdates_BeforeState.Add(method);
        }

        protected void Add_FixedUpdates_AfterState(UpdateMethod method) {

            this.FixedUpdates_AfterState.Add(method);
        }

        #endregion


        #region UpdateController

        private class UpdateController {


            #region Public Properties

            public List<UpdateMethod> Updates => m_Updates = m_Updates ?? new List<UpdateMethod>();

            #endregion


            #region Public Methods

            public void Update() {

                for (int i = 0; i < this.Updates.Count; i++) {

                    this.Updates[i]();
                }
            }

            public void Add(UpdateMethod updateMethod) {

                if (!this.Updates.Contains(updateMethod)) {

                    this.Updates.Add(updateMethod);
                }
            }

            #endregion


            #region Internal Fields

            private List<UpdateMethod> m_Updates;

            #endregion

        }

        #endregion

    }
}