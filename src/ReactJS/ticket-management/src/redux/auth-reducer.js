import {authAPI} from "../API/api";
import jwtDecode from "jwt-decode";
import React from "react";
import {stopSubmit} from "redux-form";
import {userClaim} from "../components/Constants/userConst";
import {localStorageItems} from "../components/Constants/localStorageItems";
import {formNames} from "../components/Constants/formNames";
import {messages as validateExceptionMessages} from "../components/Constants/validateExceptionMessages";

const SET_USER_DATA = 'SET_USER_DATA';
const IS_TOKEN_VALID = 'IS_TOKEN_VALID';
const SET_TOKEN = 'SET_TOKEN';

let initialState = {
    id: null,
    name: null,
    email: null,
    role: null,
    token: null,
    isAuth: false,
    isTokenValid: false,
}

const authReducer = (state = initialState, action) => {
    switch (action.type) {
        case SET_TOKEN: {
            return {
                ...state,
                token: action.token,
            }
        }
        case SET_USER_DATA: {
            return {
                ...state,
                ...action.payload,
            }
        }
        case IS_TOKEN_VALID: {
            return {
                ...state,
                isTokenValid: action.isTokenValid,
            }
        }
        default:
            return state;
    }
}
export const setToken = (token) => ({type: SET_TOKEN, token})
export const setAuthData = (id, name, email, role, isAuth) => ({
    type: SET_USER_DATA,
    payload: {id, name, email, role, isAuth}
})
export const isTokenValid = (isTokenValid) => ({type: IS_TOKEN_VALID, isTokenValid})

const setUserLocalStorage = (token) => {
    const jwtData = JSON.stringify(jwtDecode(token));
    localStorage.setItem(localStorageItems.User, jwtData);
}
const setTokenToLocalStorage = (token) => {
    localStorage.setItem(localStorageItems.Token, token);
}

let userCredential = (parsedUser) => ({
    id: parsedUser[userClaim.id],
    name: parsedUser[userClaim.name],
    email: parsedUser[userClaim.email],
    role: parsedUser[userClaim.role],
});

export const initAuthDataFromLocalStorage = (token) => (dispatch) => {
    if (!token) {
        const localToken = localStorage.getItem(localStorageItems.Token);
        if (localToken) {
            authAPI.validateToken(localToken)
                .then(response => {
                    if (response.status = "200") {
                        dispatch(isTokenValid(true));
                        setUserLocalStorage(localToken);
                        const localUser = localStorage.getItem(localStorageItems.User);
                        const parsedUser = JSON.parse(localUser);
                        let user = userCredential(parsedUser);
                        dispatch(setToken(localToken));
                        dispatch(setAuthData(user.id, user.name, user.email, user.role, true));
                    }
                });
        }
    }
}

export const login = (email, password, rememberMe) => (dispatch) => {
    authAPI.login(email, password, rememberMe)
        .then(response => {
            if (response.data) {
                let token = response.data;
                dispatch(setToken(token));
                setTokenToLocalStorage(token);
                setUserLocalStorage(token);
                const localUser = localStorage.getItem(localStorageItems.User);
                const parsedUser = JSON.parse(localUser);
                let user = userCredential(parsedUser);
                dispatch(setAuthData(user.id, user.name, user.email, user.role, true));
            } else {
                debugger;
                dispatch(stopSubmit(formNames.Login, {_error: response.data}));
            }
        }).catch(error =>  {
            if(!error.response.request.Succeeded)
            {
                dispatch(stopSubmit(formNames.Login, {_error: "Incorrect username and(or) password"}));
            }
        console.log(error);
    });
}

export const register = (userName, firstName, surname, email, password, passwordConfirm) => (dispatch) => {
    authAPI.register(userName, firstName, surname, email, password, passwordConfirm)
        .then(response => {
            if (response.data) {
                let token = response.data;
                dispatch(setToken(token));
                setTokenToLocalStorage(token);
                setUserLocalStorage(token);
                const localUser = localStorage.getItem(localStorageItems.User);
                const parsedUser = JSON.parse(localUser);
                let user = userCredential(parsedUser);
                dispatch(setAuthData(user.id, user.name, user.email, user.role, true));
            } else {
                debugger;
                dispatch(stopSubmit(formNames.Login, {_error: response.data}));
            }
        }).catch(error =>  {
            debugger;
        if(error.response.data)
        {
            if (error.response.data.includes(validateExceptionMessages.PasswordTooShort)) {
                dispatch(stopSubmit(formNames.Register, {password: validateExceptionMessages.PasswordTooShort}));
            }
            if (error.response.data.includes(validateExceptionMessages.PasswordRequiresNonAlphanumeric)) {
                dispatch(stopSubmit(formNames.Register, {password: validateExceptionMessages.PasswordRequiresNonAlphanumeric}));
            }
            if (error.response.data.includes(validateExceptionMessages.PasswordRequiresDigit)) {
                dispatch(stopSubmit(formNames.Register, {password: validateExceptionMessages.PasswordRequiresDigit}));
            }
            if (error.response.data.includes(validateExceptionMessages.PasswordRequiresUpper)) {
                dispatch(stopSubmit(formNames.Register, {password: validateExceptionMessages.PasswordRequiresUpper}));
            }
            if (error.response.data.includes(validateExceptionMessages.PasswordConfirm)) {
                dispatch(stopSubmit(formNames.Register, {passwordConfirm: validateExceptionMessages.PasswordConfirm}));
            }
        }
        console.log(error);
    });
}

export const logout = () => (dispatch) => {
    dispatch(setToken(null, null, null, null));
    dispatch(setAuthData(null));
    localStorage.removeItem(localStorageItems.Token)
    localStorage.removeItem(localStorageItems.User)
}
export default authReducer;
