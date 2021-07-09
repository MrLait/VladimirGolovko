import React from "react";
import style from "./EventsAreas.module.css"
import {useTranslation} from "react-i18next";
import {seatStates} from "../Constants/seatConst";
import Preloader from "../../common/Preloaders/Preloader";
import {removeItem} from "../../redux/eventAreas-reducer";

let EventAreas = (props) => {
    const {t} = useTranslation();
    if (!props.eventAreas) return <Preloader/>
    return (
        <div className={style.outer}>
            {props.eventAreas.map(e =>
                <div key={e.id}>
                    <div>
                        {e.description}
                    </div>
                    <span>
                        {e.eventSeats.map(s =>
                            s.state === seatStates.Available
                                ?
                                <button disabled={props.clickingInProgress.some(id => id === s.id)}
                                        className={style.seat + ' ' + style.available} data-title={`Price ${e.price}`}
                                        key={s.id} onClick={() => { props.addItem(e.id, s.id);}}>
                                        {s.id}
                                </button>
                                :
                                s.state === seatStates.Booked
                                    ?
                                    <button disabled={props.clickingInProgress.some(id => id === s.id)}
                                            className={style.seat + ' ' + style.booked} data-title={`Price ${e.price}`}
                                            key={s.id} onClick={() => { props.removeItem(e.id, s.id);}}>
                                        {s.id}
                                    </button>
                                    :
                                    s.state === seatStates.Purchased
                                        ?
                                        <button disabled={true} className={style.seat + ' ' + style.purchased}
                                                data-title={`Price ${e.price}`} key={s.id}>
                                            {s.id}
                                        </button>
                                        : ""
                        )}
                    </span>
                </div>
            )}
        </div>
    )
}
export default EventAreas;