import {purchaseHistoryAPI} from "../API/api";
import {userClaim} from "../components/Constants/userConst";

const TOGGLE_IS_FETCHING = 'TOGGLE_IS_FETCHING';
const SET_ITEMS = 'SET_ITEMS';
const SET_USER_ID = 'SET_SET_USER_ID';

let initialState = {
    id: null,
    isFetching: true,
    items: []
}

const purchaseHistoryReducer = (state = initialState, action) => {
    switch (action.type) {
        case SET_USER_ID: {
            return {...state, id: action.id}
        }
        case TOGGLE_IS_FETCHING: {
            return {...state, isFetching: action.isFetching}
        }
        case SET_ITEMS: {
            return {
                ...state,
                items: action.items
            }
        }
        default:
            return state;
    }
}

export const toggleIsFetching = (isFetching) => ({type: TOGGLE_IS_FETCHING, isFetching})
export const setItems = (items) => ({type: SET_ITEMS, items})
export const setUserId = (id) => ({type: SET_USER_ID, id})

export const getUserItems = () => (dispatch) => {
    const id = userId();
    purchaseHistoryAPI.getUserItems(id)
        .then(response => {
            dispatch(toggleIsFetching(false));
            dispatch(setItems(response.data.items));
        })
}

const userId = () => {
    const localUser = localStorage.getItem('user');
    const parsedUser = JSON.parse(localUser);
    let userCredential = (parsedUser) => ({
        id: parsedUser[userClaim.id],
        name: parsedUser[userClaim.name],
        email: parsedUser[userClaim.email],
        role: parsedUser[userClaim.role],
    });
    let id = userCredential(parsedUser).id;
    return id;
}



export default purchaseHistoryReducer;
