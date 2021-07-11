import {eventAPI} from "../API/api";

const SET_EVENTS = 'SET_EVENTS';
const TOGGLE_IS_FETCHING = 'TOGGLE_IS_FETCHING';

let initialState = {
    events: [],
    isFetching: true
}

const eventsReducer = (state = initialState, action) => {
    switch (action.type){
        case SET_EVENTS: {
            return {...state, events: action.events}
        }
        case TOGGLE_IS_FETCHING: {
                return { ...state, isFetching: action.isFetching }
        }
        default:
            return state;
    }
}

export const setEvents = (events) => ({type: SET_EVENTS, events})
export const toggleIsFetching = (isFetching) => ({type: TOGGLE_IS_FETCHING, isFetching})

export const getEvents = () => (dispatch) => {
    dispatch(toggleIsFetching(true));
    eventAPI.getEvents()
        .then(response => {
            dispatch(setEvents(response.data));
            dispatch(toggleIsFetching(false));
        })
    }

export default eventsReducer;
