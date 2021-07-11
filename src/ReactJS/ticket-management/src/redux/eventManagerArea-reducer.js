import {eventAPI} from "../API/api";

const TOGGLE_IS_FETCHING = 'TOGGLE_IS_FETCHING';
const SET_EVENTS = 'SET_EVENTS';
const SET_IN_PROGRESS = 'SET_IN_PROGRESS';

let initialState = {
    id: null,
    isFetching: false,
    events: [],
    inProgress: false,
}

const eventManagerAreaReducer = (state = initialState, action) => {
    switch (action.type) {
        case TOGGLE_IS_FETCHING: {
            return {...state, isFetching: action.isFetching}
        }
        case SET_IN_PROGRESS: {
            return {...state, inProgress: action.inProgress}
        }
        case SET_EVENTS: {
            return {...state, events: action.events}
        }
        default:
            return state;
    }
}

export const setInProgress = (inProgress) => ({type: SET_IN_PROGRESS, inProgress})
export const toggleIsFetching = (isFetching) => ({type: TOGGLE_IS_FETCHING, isFetching})
export const setEvents = (events) => ({type: SET_EVENTS, events})

export const getEvents = () => (dispatch) => {
    dispatch(toggleIsFetching(true));
    eventAPI.getEvents()
        .then(response => {
            dispatch(setEvents(response.data));
            dispatch(toggleIsFetching(false));
        })
}

export default eventManagerAreaReducer;
