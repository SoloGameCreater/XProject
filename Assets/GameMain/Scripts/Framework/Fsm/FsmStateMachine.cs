// using DragonU3DSDK;
// using UnityEngine;

// public class FsmStateMachine<entityType>
// {
//     private entityType mOwner;

//     private FsmState<entityType> mCurrentState;
//     private FsmState<entityType> mPreviouState;

// 	private bool mIsChangingState = false;

//     public FsmStateMachine(entityType owner)
//     {
//         mOwner = owner;
//         mCurrentState = null;
//         mPreviouState = null;
//     }

//     public void InitCurrentState(FsmState<entityType> currentState, params object[] objs)
//     {
//         if (currentState != null)
//         {
//             mCurrentState = currentState;
//             mCurrentState.mTarget = mOwner;
//             mCurrentState.Enter(objs);
//         }
//         else
//         {
//             Debug.LogError("can't set null state");
//         }
//     }

//     public void FsmUpdate(float deltaTime)
//     {
//         if (mCurrentState != null)
//         {
//             mCurrentState.Update(deltaTime);
//         }
//     }

//     public void ChangeState(FsmState<entityType> newState, params object[] objs)
//     {
//         if (newState == null)
//         {
//             Debug.LogError("can't use null state");
//         }

// 		if (mIsChangingState)
// 		{
// 			Debug.LogError("state is changing. ignore other changes");
// 			return;
// 		}

// 		mIsChangingState = true;

// 		Debug.LogError("state starts to change");
// 		mCurrentState.Exit();

//         mPreviouState = mCurrentState;

//         mCurrentState = newState;

//         mCurrentState.mTarget = mOwner;

//         mCurrentState.Enter(objs);

// 		mIsChangingState = false;
// 		Debug.LogError("state finished changing");
// 	}

//     public void ReverToPreviousState()
//     {
//         this.ChangeState(mPreviouState);
//     }

//     public FsmState<entityType> GetCurrentState()
//     {
//         return mCurrentState;
//     }

//     public FsmState<entityType> GetPreviousState()
//     {
//         return mPreviouState;
//     }
// }