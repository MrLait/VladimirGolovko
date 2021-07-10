import {getCurUserId} from "../common/Services/UserService";
import {profileAPI} from "../API/api";
import {stopSubmit} from "redux-form";
import i18n from "i18next";

const SET_BALANCE = 'SET_BALANCE';
const SET_SURNAME = 'SET_SURNAME';
const SET_EMAIL = 'SET_EMAIL';
const SET_LANGUAGE = 'SET_LANGUAGE';
const SET_TIME_ZONE_OFFSET = 'SET_TIME_ZONE_OFFSET';
const SET_FIRST_NAME = 'SET_FIRST_NAME';
const SET_NEW_PASSWORD = 'SET_NEW_PASSWORD';
const TOGGLE_IS_FETCHING = 'TOGGLE_IS_FETCHING';
const toggleIsClickingInProgress = 'toggleIsClickingInProgress';

let initialState = {
    balance: null,
    isFetching: false,
    clickingInProgress: false,
    firstName: null,
    surname: null,
    email: null,
    language: null,
    timeZoneOffSet: null,
    isNewPassword: false,
}

const profileReducer = (state = initialState, action) => {
    switch (action.type) {

        case SET_SURNAME: {
            return {...state, surname: action.surname}
        }
        case SET_EMAIL: {
            return {...state, email: action.email}
        }
        case SET_LANGUAGE: {
            return {...state, language: action.language}
        }
        case SET_TIME_ZONE_OFFSET: {
            return {...state, timeZoneOffSet: action.timeZoneOffSet}
        }
        case SET_FIRST_NAME: {
            return {...state, firstName: action.firstName}
        }
        case SET_NEW_PASSWORD: {
            return {...state, isNewPassword: action.isNewPassword}
        }
        case SET_BALANCE: {
            return {...state, balance: action.balance}
        }
        case TOGGLE_IS_FETCHING: {
            return {...state, isFetching: action.isFetching}
        }
        case toggleIsClickingInProgress: {
            return {...state, clickingInProgress: action.clickingInProgress}
        }
        default:
            return state;
    }
}

export const setSurname = (surname) => ({type: SET_SURNAME, surname})
export const setEmail = (email) => ({type: SET_EMAIL, email})
export const setLanguage = (language) => ({type: SET_LANGUAGE, language})
export const setTimeZoneOffSet = (timeZoneOffSet) => ({type: SET_TIME_ZONE_OFFSET, timeZoneOffSet})
export const setFirstName = (firstName) => ({type: SET_FIRST_NAME, firstName})
export const setNewPassword = (isNewPassword) => ({type: SET_NEW_PASSWORD, isNewPassword})
export const setBalance = (balance) => ({type: SET_BALANCE, balance})
export const toggleIsFetching = (isFetching) => ({type: TOGGLE_IS_FETCHING, isFetching})
export const toggleClickingInProgress = (clickingInProgress) => ({type: toggleIsClickingInProgress, clickingInProgress})

export const getUserProfile = () => (dispatch) => {
    dispatch(toggleIsFetching(true));
    const userId = getCurUserId();
    profileAPI.getProfile(userId)
        .then(response => {
            dispatch(setSurname(response.data.surname));
            dispatch(setEmail(response.data.email));
            dispatch(setLanguage(response.data.language));
            dispatch(setTimeZoneOffSet(response.data.timeZoneOffset));
            dispatch(setFirstName(response.data.firstName));
            dispatch(setBalance(Number(response.data.balance)));
            i18n.changeLanguage(response.data.language);
            dispatch(toggleIsFetching(false));
        })
}

export const getBalance = () => (dispatch) => {
    dispatch(toggleIsFetching(true));
    const userId = getCurUserId();
    profileAPI.getBalance(userId)
        .then(response => {
            dispatch(setBalance(Number(response.data)));
            dispatch(toggleIsFetching(false));
        })
}
export const editSurname = (surname) => (dispatch) => {
    dispatch(toggleClickingInProgress(true))
    const userId = getCurUserId();
    profileAPI.editSurname(userId, surname)
        .then(response => {
            debugger;
            dispatch(setSurname(response.data));
            dispatch(toggleClickingInProgress(false))
        })
}

export const editFirstName = (firstName) => (dispatch) => {
    dispatch(toggleClickingInProgress(true))
    const userId = getCurUserId();
    profileAPI.editFirstName(userId, firstName)
        .then(response => {
            debugger;
            dispatch(setFirstName(response.data));
            dispatch(toggleClickingInProgress(false))
        })
}
export const editEmail = (email) => (dispatch) => {
    dispatch(toggleClickingInProgress(true))
    const userId = getCurUserId();
    profileAPI.editEmail(userId, email)
        .then(response => {
            debugger;
            dispatch(setEmail(response.data));
            dispatch(toggleClickingInProgress(false))
        })
}

export const editPassword = (oldPassword, newPassword) => (dispatch) => {
    dispatch(toggleClickingInProgress(true))
    dispatch(setNewPassword(false));
    debugger;
    const userId = getCurUserId();
    profileAPI.editPassword(userId, oldPassword, newPassword)
        .then(response => {
            debugger;
            dispatch(setNewPassword(response.data));
            dispatch(toggleClickingInProgress(false))
        }).catch(error =>  {
        debugger;
        if(error.response.data)
        {
            if (error.response.data.includes('PasswordMismatch')) {
                dispatch(stopSubmit('editPassword', {oldPassword: "PasswordMismatch"}));
            }
            if (error.response.data.includes('PasswordTooShort')) {
                dispatch(stopSubmit('editPassword', {newPassword: "PasswordTooShort"}));
            }
            if (error.response.data.includes('PasswordRequiresNonAlphanumeric')) {
                dispatch(stopSubmit('editPassword', {newPassword: "PasswordRequiresNonAlphanumeric"}));
            }
            if (error.response.data.includes('PasswordRequiresDigit')) {
                dispatch(stopSubmit('editPassword', {newPassword: "PasswordRequiresDigit"}));
            }
            if (error.response.data.includes('PasswordRequiresUpper')) {
                dispatch(stopSubmit('editPassword', {newPassword: "PasswordRequiresUpper"}));
            }
            if (error.response.data.includes('PasswordConfirm')) {
                dispatch(stopSubmit('editPassword', {newPassword: "PasswordConfirm"}));
            }
            dispatch(toggleClickingInProgress(false))
        }
        });
}

export const editTimeZoneOffset = (timeZoneOffset) => (dispatch) => {
    dispatch(toggleClickingInProgress(true))
    const userId = getCurUserId();
    debugger;
    profileAPI.editTimeZoneOffset(userId, timeZoneOffset)
        .then(response => {
            debugger;
            dispatch(setTimeZoneOffSet(timeZoneOffset));
            dispatch(toggleClickingInProgress(false))
        })
}

export const editLanguage = (culture) => (dispatch) => {
    dispatch(toggleClickingInProgress(true))
    const userId = getCurUserId();
    profileAPI.setLanguage(userId, culture)
        .then(response => {
            debugger;
            localStorage.setItem('i18nextLng', culture);
            dispatch(setLanguage(culture));
            dispatch(toggleClickingInProgress(false))
        })
}

export const deposit = (balance) => (dispatch) => {
    dispatch(toggleClickingInProgress(true))
    const userId = getCurUserId();
    profileAPI.getBalance(userId)
        .then(response => {
            const curBalance = response.data;
            const updatedBalance = Number(curBalance) + Number(balance);
            debugger;
            profileAPI.updateBalance(userId, updatedBalance)
                .then(() => {
                    dispatch(setBalance(updatedBalance));
                    dispatch(toggleClickingInProgress(false))
                })

        })
}

export default profileReducer;
