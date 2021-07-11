import React from "react";
import {connect} from "react-redux";
import CreateEventForm from "./CreateEventForm";
import Preloader from "../../../../common/Preloaders/Preloader";
import {createEvent, setEventInProgress, toggleIsFetching} from "../../../../redux/eventManagerArea-reducer";

class CreateEventContainer extends React.Component {
    componentDidMount() {
        this.props.toggleIsFetching(true);
        this.props.setEventInProgress(true);
    }

    render() {
        return <>
            {this.props.isFetching ? <Preloader/>
                : <CreateEventForm {...this.props}
                                   onSubmit={(formData) => {
                                       this.props.createEvent(formData)}}/>
            }
        </>
    }
}

let mapStateToProps = (state) => {
    return {
        isAuth: state.authPage.isAuth,
        events: state.eventManagerAreaPage.events,
        event: state.eventManagerAreaPage.event,
        isFetching: state.eventManagerAreaPage.isFetching,
        creteEventInProgress: state.eventManagerAreaPage.creteEventInProgress,
        isCreateEventSuccessful: state.eventManagerAreaPage.isCreateEventSuccessful,
    }
}

export default connect(
    mapStateToProps,
    {
        createEvent,
        setEventInProgress,
        toggleIsFetching
    })(CreateEventContainer);