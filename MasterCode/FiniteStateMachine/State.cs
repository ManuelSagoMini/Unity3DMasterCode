namespace MasterCode {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    public class State<T> where T : UnityEngine.MonoBehaviour {

        #region Virtual Methods

        virtual public void Enter() { }

        virtual public void Exit() { }

        #endregion


        #region Public Methods

        public void Bind(FiniteStateMachine<T> finiteStateMachine) {

            m_FSM = finiteStateMachine;
        }

        public void Update() {

            this.Updates.Update();
        }

        public void LateUpdate() {

            this.LateUpdates.Update();
        }

        public void FixedUpdate() {

            this.FixedUpdates.Update();
        }

        public void ClearUpdates() {

            this.Updates.Clear();
            this.LateUpdates.Clear();
            this.FixedUpdates.Clear();
        }

        #endregion


        #region Internal Fields

        protected delegate void UpdateMethod();
        private FiniteStateMachine<T> m_FSM;
        private UpdateController m_Updates;
        private UpdateController m_LateUpdates;
        private UpdateController m_FixedUpdates;


        #endregion


        #region Internal Properties

        protected T Root => m_FSM.Root;
        private FiniteStateMachine<T> FSM => m_FSM;
        private UpdateController Updates => m_Updates = m_Updates ?? new UpdateController();
        private UpdateController LateUpdates => m_LateUpdates = m_LateUpdates ?? new UpdateController();
        private UpdateController FixedUpdates => m_FixedUpdates = m_FixedUpdates ?? new UpdateController();

        #endregion


        #region Internal Methods

        protected void ChangeState<W>() where W : State<T> {

            this.FSM.ChangeState<W>();
        }

        protected void Add_Update(UpdateMethod updateMethod) {

            this.Updates.Add(updateMethod);
        }

        protected void Remove_Update(UpdateMethod updateMethod) {

            this.Updates.Remove(updateMethod);
        }

        protected void Add_LateUpdate(UpdateMethod updateMethod) {

            this.LateUpdates.Add(updateMethod);
        }

        protected void Remove_LateUpdate(UpdateMethod updateMethod) {

            this.LateUpdates.Remove(updateMethod);
        }

        protected void Add_FixedUpdate(UpdateMethod updateMethod) {

            this.FixedUpdates.Add(updateMethod);
        }

        protected void Remove_FixedUpdate(UpdateMethod updateMethod) {

            this.FixedUpdates.Remove(updateMethod);
        }

        #endregion


        #region UpdateController

        private class UpdateController {

            #region Public Methods

            public void Add(UpdateMethod updateMethod) {

                if (!this.Updates.Contains(updateMethod) && !this.Update_Add.Contains(updateMethod)) {

                    this.Update_Add.Add(updateMethod);
                }
            }

            public void Remove(UpdateMethod updateMethod) {

                if (this.Updates.Contains(updateMethod) && !this.Update_Remove.Contains(updateMethod)) {

                    this.Update_Remove.Add(updateMethod);
                }
            }

            public void Update() {

                this.Refresh();

                for (int i = 0; i < this.Updates.Count; i++) {

                    UpdateMethod updateMethod;
                    updateMethod = this.Updates[i];

                    if (!this.Update_Remove.Contains(updateMethod)) {

                        updateMethod();
                    }
                }
            }

            public void Clear() {

                this.Updates.Clear();
            }

            #endregion


            #region Internal Fields

            private List<UpdateMethod> m_Updates;
            private List<UpdateMethod> m_Update_Add;
            private List<UpdateMethod> m_Update_Remove;

            #endregion


            #region Internal Properties

            private List<UpdateMethod> Updates => m_Updates = m_Updates ?? new List<UpdateMethod>();
            private List<UpdateMethod> Update_Add => m_Update_Add = m_Update_Add ?? new List<UpdateMethod>();
            private List<UpdateMethod> Update_Remove => m_Update_Remove = m_Update_Remove ?? new List<UpdateMethod>();

            #endregion


            #region Internal Methods

            private void Refresh() {

                if (this.Update_Add.Count > 0) {

                    for (int i = 0; i < this.Update_Add.Count; i++) {

                        UpdateMethod updateMethod;
                        updateMethod = this.Update_Add[i];
                        this.Updates.Add(updateMethod);
                    }

                    this.Update_Add.Clear();
                }

                if (this.Update_Remove.Count > 0) {

                    for (int i = 0; i < this.Update_Remove.Count; i++) {

                        UpdateMethod updateMethod;
                        updateMethod = this.Update_Remove[i];
                        this.Updates.Remove(updateMethod);
                    }

                    this.Update_Remove.Clear();
                }
            }

            #endregion

        }

        #endregion

    }
}