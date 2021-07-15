import React from "react";
import {connect} from "react-redux";
import Preloader from "../../../common/Preloaders/Preloader";
import EventManagerArea from "./EventManagerArea";
import {
    deleteEvent,
    getEvents, setIsCreateEventSuccessful, setIsUpdateEventSuccessful
} from "../../../redux/eventManagerArea-reducer";
import {ifNotAuthRedirectToHome} from "../../../hoc/ifNotAuthRedirectToHome";
import {compose} from "redux";

class EventManagerAreaContainer extends React.Component {
    componentDidMount() {
        this.props.getEvents();
        this.props.setIsCreateEventSuccessful(false);
        this.props.setIsUpdateEventSuccessful(false);
        debugger;
    }

    render() {
        return <>
            {this.props.isFetching ? <Preloader/> : <EventManagerArea {...this.props}/>}
        </>
    }
}

let mapStateToProps = (state) => {
    return {
        events: state.eventManagerAreaPage.events,
        isFetching: state.eventManagerAreaPage.isFetching,
        isDeleteError: state.eventManagerAreaPage.isDeleteError,
    }
}

export default compose(
    connect(
        mapStateToProps,
        {
            getEvents, setIsCreateEventSuccessful, setIsUpdateEventSuccessful,
            deleteEvent
        }),
    ifNotAuthRedirectToHome
)(EventManagerAreaContainer);