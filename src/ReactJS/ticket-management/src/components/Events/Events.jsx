import React from "react";
import {Link } from "react-router-dom";

let Events = (props) => {
    return(
        <div>
            {
                props.events.map(e => <div key={e.id}>
                    <Link to={"/eventArea/id=" + e.id }>
                        <div> {e.description}</div>
                    </Link >
                </div>)
            }
        </div>
    )
}
export default Events;