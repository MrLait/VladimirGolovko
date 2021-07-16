import {basketAPI, eventAreaAPI} from "../API/api";
import {seatStates} from "../components/Constants/seatConst";
import {getCurUserId} from "../common/Services/UserService";

const SET_EVENT_AREAS = 'SET_EVENT_AREAS';
const TOGGLE_IS_FETCHING = 'TOGGLE_IS_FETCHING';
const toggleIsClickingInProgress = 'toggleIsClickingInProgress';
const ADD_SUCCESS = 'ADD_SUCCESS';
const REMOVE_SUCCESS = 'REMOVE_SUCCESS';

let initialState = {
    eventAreas: null,
    isFetching: false,
    clickingInProgress: []
}

const eventAreasReducer = (state = initialState, action) => {
    switch (action.type) {
        case SET_EVENT_AREAS: {
            return {...state, eventAreas: action.eventAreas}
        }
        case ADD_SUCCESS: {
            return {
                ...state,
                eventAreas: state.eventAreas.map(ea => {
                        if (ea.id === action.eventAreaId) {
                            return {
                                ...ea,
                                eventSeats: ea.eventSeats.map(s => {
                                    if (s.id === action.seatId) {
                                        return {...s, state: seatStates.Booked}
                                    }
                                    return s;
                                })
                            }
                        }
                        return ea;
                    }
                )
            }
        }
        case REMOVE_SUCCESS: {
            return {
                ...state,
                eventAreas: state.eventAreas.map(ea => {
                        if (ea.id === action.eventAreaId) {
                            return {
                                ...ea,
                                eventSeats: ea.eventSeats.map(s => {
                                    if (s.id === action.seatId) {
                                        return {...s, state: seatStates.Available}
                                    }
                                    return s;
                                })
                            }
                        }
                        return ea;
                    }
                )
            }
        }
        case TOGGLE_IS_FETCHING: {
            return {...state, isFetching: action.isFetching}
        }
        case toggleIsClickingInProgress: {
            return {
                ...state,
                clickingInProgress: action.isFetching
                    ? [...state.clickingInProgress, action.id]
                    : state.clickingInProgress.filter(id => id !== action.id)
            }
        }
        default:
            return state;
    }
}

export const addSeatItem = (eventAreaId, seatId) => ({type: ADD_SUCCESS, eventAreaId, seatId})
export const removeSeatItem = (eventAreaId, seatId) => ({type: REMOVE_SUCCESS, eventAreaId, seatId})
export const setEventAreas = (eventAreas) => ({type: SET_EVENT_AREAS, eventAreas})
export const toggleIsFetching = (isFetching) => ({type: TOGGLE_IS_FETCHING, isFetching})
export const toggleClickingInProgress = (isFetching, id) => ({type: toggleIsClickingInProgress, isFetching, id})

export const getEventAreas = (eventId) => (dispatch) => {
    eventAreaAPI.getEventAreas(eventId).then(response => {
        dispatch(setEventAreas(response.data));
        dispatch(toggleIsFetching(false));
    })
}

export const addItem = (eventAreaId, itemId) => (dispatch) => {
    dispatch(toggleClickingInProgress(true, itemId))
    const id = getCurUserId();
    basketAPI.addItemToUserBasket(id, itemId)
        .then(() => {
            dispatch(addSeatItem(eventAreaId, itemId))
            dispatch(toggleClickingInProgress(false, itemId));
        })
}

export const removeItem = (eventAreaId, itemId) => (dispatch) => {
    dispatch(toggleClickingInProgress(true, itemId))
    const id = getCurUserId();
    basketAPI.deleteItemFromUserBasket(id, itemId)
        .then(() => {
            dispatch(removeSeatItem(eventAreaId, itemId))
            dispatch(toggleClickingInProgress(false, itemId));
        })
}

export default eventAreasReducer;
