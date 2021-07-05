const SET_EVENT_AREAS = 'SET_EVENT_AREAS';
const TOGGLE_IS_FETCHING = 'TOGGLE_IS_FETCHING';

let initialState = {
    eventAreas: null,
    isFetching: true
}

const eventAreasReducer = (state = initialState, action) => {
    switch (action.type){
        case SET_EVENT_AREAS: {
            return {...state, eventAreas: action.eventAreas}
        }
        case TOGGLE_IS_FETCHING: {
                return { ...state, isFetching: action.isFetching }
        }
        default:
            return state;
    }
}

export const setEventAreas = (eventAreas) => ({type: SET_EVENT_AREAS, eventAreas})
export const toggleIsFetching = (isFetching) => ({type: TOGGLE_IS_FETCHING, isFetching})

export default eventAreasReducer;
