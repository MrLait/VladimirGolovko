import React from "react";
import EventAreas from "./EventAreas";
import {connect} from "react-redux";
import {
    addItem,
    getEventAreas,
    removeItem,
    toggleClickingInProgress,
    toggleIsFetching
} from "../../redux/eventAreas-reducer";
import {withRouter} from "react-router";
import {compose} from "redux";
import Preloader from "../../common/Preloaders/Preloader";

class EventAreasContainer extends React.Component {
    componentDidMount() {
        let eventId = this.props.match.params.eventId;
        this.props.toggleIsFetching(true);
        this.props.toggleIsFetching(true);
        this.props.getEventAreas(eventId);
    }
    render() {
        return <>
            {this.props.isFetching
                ? <Preloader/>
                : null}
            <EventAreas {...this.props}/>
        </>
    }
}

let mapStateToProps = (state) => ({
    isFetching: state.eventAreasPage.isFetching,
    eventAreas: state.eventAreasPage.eventAreas,
    clickingInProgress: state.eventAreasPage.clickingInProgress,
    isAuth: state.authPage.isAuth
});

export default compose(
    connect(mapStateToProps,
        {toggleClickingInProgress, toggleIsFetching, getEventAreas, addItem, removeItem}),
    withRouter,
)(EventAreasContainer)
