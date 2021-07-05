import React from "react";
import {connect} from "react-redux";
import * as axios from "axios";
import Preloader from "../../common/Preloaders/Preloader";

let EventAreas = (props) => {
    if (!props.eventAreas){
        return <Preloader/>
    }
    return <div> {props.eventAreas[0].id} </div>
}

export default EventAreas;