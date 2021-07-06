import React from "react";
import {Link } from "react-router-dom";

let Events = (props) => {
    return(
        <div>
            {
                props.events.map(e => <div key={e.id}>
                        <div> {e.description}</div>
                    <Link to={"/eventArea/id=" + e.id }>
                        <button> click </button>
                    </Link >
                </div>)
            }
        </div>
    )
}
export default Events;