import {purchaseHistoryAPI} from "../API/api";
import {getCurUserId} from "../common/Services/UserService";

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
    const id = getCurUserId();
    purchaseHistoryAPI.getUserItems(id)
        .then(response => {
            dispatch(toggleIsFetching(false));
            dispatch(setItems(response.data.items));
        })
}

export default purchaseHistoryReducer;
