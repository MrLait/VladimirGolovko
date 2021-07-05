const SET_USER_DATA = 'SET_USER_DATA';
const IS_TOKEN_VALID = 'IS_TOKEN_VALID';

let initialState = {
    token: null,
    isAuth: false,
    isTokenValid: false
}

const authReducer = (state = initialState, action) => {
    switch (action.type){
        case SET_USER_DATA: {
            return {...state,
                token: action.token}
        }
        case IS_TOKEN_VALID: {
            return {...state,
                isTokenValid: action.isTokenValid,
                isAuth: true}
        }
        default:
            return state;
    }
}

export const setAuthData = (token) => ({type: SET_USER_DATA, token})
export const isTokenValid = (isTokenValid) => ({type: IS_TOKEN_VALID, isTokenValid})

export default authReducer;
