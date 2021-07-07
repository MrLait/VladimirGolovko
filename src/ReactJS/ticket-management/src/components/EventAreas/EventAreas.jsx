import React from "react";
import Preloader from "../../common/Preloaders/Preloader";
import style from "./EventsAreas.module.css"

let EventAreas = (props) => {
    if (!props.eventAreas) {
        return <Preloader/>
    }
    return (
        <div className={style.outer}>
            {props.eventAreas.map(
                e =>
                    <div key={e.id}>
                        <div>
                            {e.description}
                        </div>
                        <span>{e.eventSeats.map(
                            s =>
                                <button className={style.seat} data-title={`Price ${e.price}`} key={s.id}>{s.id}</button>
                        )}
                        </span>
                    </div>
            )}
        </div>
    )
}

export default EventAreas;