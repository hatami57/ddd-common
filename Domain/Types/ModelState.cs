using System;
using System.Collections.Generic;
using System.Text;

namespace DDDCommon.Domain.Types
{
    public enum ModelStates
    {
        None = 0,
        Added = 1,
        Updated = 2,
        Removed = 3
    }
    public class ModelState<T> : ValueObject
    {
        public T Model { get; }
        public ModelStates State { get; }

        public ModelState(T model, ModelStates state = ModelStates.None)
        {
            Model = model;
            State = state;
        }

        public ModelState<T> NewState(ModelStates newState)
        {
            return new ModelState<T>(Model, newState);
        }
    }
}
