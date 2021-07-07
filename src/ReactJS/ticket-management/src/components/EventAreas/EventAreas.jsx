import React from "react";
import Preloader from "../../common/Preloaders/Preloader";

let EventAreas = (props) => {
    if (!props.eventAreas){
        return <Preloader/>
    }
    return (
        <div>
            {
            props.eventAreas.map( e => <div key={e.id}>
                <div>{e.description}</div>
                <div>
                    {
                        e.eventSeats.map(s => <div key={s.id}>
                        <div>{s.id}</div>
                        </div>)
                    }
                </div>
                </div>)
            }
        </div>
    )
}

export default EventAreas;