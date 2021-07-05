import React from "react";
import EventAreas from "./EventAreas";
import * as axios from "axios";
import {connect} from "react-redux";
import {setEventAreas} from "../../redux/eventAreas-reducer";
import  {withRouter} from "react-router";

class EventAreasContainer extends React.Component {
    componentDidMount() {
        let eventId = this.props.match.params.eventId;
        axios.get("https://localhost:5003/EventArea?" + eventId)
            .then(response => {
            this.props.setEventAreas(response.data);
        })
    }
    render () {
        return <EventAreas {...this.props} eventAreas={this.props.eventAreas}/>
    }
}
let mapStateToProps = (state) => ({
    eventAreas: state.eventAreasPage.eventAreas
});

let WithUrlDataEventAreasContainer = withRouter(EventAreasContainer);

export default connect(mapStateToProps,
    {
        setEventAreas
    }) (WithUrlDataEventAreasContainer);