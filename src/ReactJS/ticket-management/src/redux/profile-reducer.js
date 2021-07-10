import {getCurUserId} from "../common/Services/UserService";
import {profileAPI} from "../API/api";

const SET_BALANCE = 'SET_BALANCE';
const TOGGLE_IS_FETCHING = 'TOGGLE_IS_FETCHING';
const toggleIsClickingInProgress = 'toggleIsClickingInProgress';

let initialState = {
    balance: null,
    isFetching: false,
    clickingInProgress: false
}

const profileReducer = (state = initialState, action) => {
    switch (action.type){
        case SET_BALANCE: {
            return {...state, balance: action.balance}
        }
        case TOGGLE_IS_FETCHING: {
                return { ...state, isFetching: action.isFetching }
        }
        case toggleIsClickingInProgress: {
            return { ...state, clickingInProgress: action.clickingInProgress }
        }
        default:
            return state;
    }
}

export const setBalance = (balance) => ({type: SET_BALANCE, balance})
export const toggleIsFetching = (isFetching) => ({type: TOGGLE_IS_FETCHING, isFetching})
export const toggleClickingInProgress = (clickingInProgress) => ({type: toggleIsClickingInProgress, clickingInProgress})

export const getBalance = () => (dispatch) => {
    dispatch(toggleIsFetching(true));
    const userId = getCurUserId();
    profileAPI.getBalance(userId)
        .then(response => {
            dispatch(setBalance(response.data));
            dispatch(toggleIsFetching(false));
        })
    }

export const deposit = (balance) => (dispatch) => {
    dispatch(toggleClickingInProgress(true))
    const userId = getCurUserId();
    profileAPI.getBalance(userId)
        .then(response => {
            const curBalance = response.data;
            const updatedBalance = parseFloat(curBalance) + parseFloat(balance);
            debugger;
            profileAPI.updateBalance(userId, updatedBalance)
                .then(response => {
                    dispatch(setBalance(updatedBalance));
                    dispatch(toggleClickingInProgress(false))
                })

        })
}

export default profileReducer;
