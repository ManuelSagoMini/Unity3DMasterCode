namespace MasterCode {

    using UnityEngine;
    using System.Collections.Generic;
    using System.Collections;
    using System;

    public class FiniteStateMachine<T> where T : UnityEngine.MonoBehaviour {

        public FiniteStateMachine(T mono) {

            m_Root = mono;
        }

        #region Public Properties

        public T Root => m_Root;
        public List<State<T>> States => m_States = m_States ?? new List<State<T>>();
        public State<T> State { get; private set; }

        #endregion


        #region Public Methods

        public void Add(State<T> state) {

            if (!this.HasState(state)) {

                state.Bind(this);
                this.States.Add(state);
            }
        }

        public void ChangeState(int index) {

            if (index >= 0 && index < this.States.Count) {

                this.ChangeState(this.States[index].GetType());
            }
        }

        public void ChangeState<W>() where W : State<T> {

            Type type;
            type = typeof(W);
            this.ChangeState(type);
        }

        public void Update() {

            if (this.State != null) {

                this.State.Update();
            }
        }

        public void LateUpdate() {

            if (this.State != null) {

                this.State.LateUpdate();
            }
        }

        public void FixedUpdate() {

            if (this.State != null) {

                this.State.FixedUpdate();
            }
        }

        #endregion


        #region Internal Fields

        private List<State<T>> m_States;

        private T m_Root;

        #endregion


        #region Internal Methods

        private void ChangeState(Type type) {

            if (this.State.GetType().IsAssignableFrom(type)) {

                return;
            }

            for (int i = 0; i < this.States.Count; i++) {

                State<T> state;
                state = this.States[i];

                if (state != this.State && state.GetType().IsAssignableFrom(type)) {

                    if (this.State != null) {

                        this.State.ClearUpdates();
                        this.State.Exit();
                    }

                    this.State = state;
                    this.State.Enter();
                }
            }
        }

        private bool HasState(State<T> state) {

            return this.States.Contains(state);
        }

        #endregion
    }
}