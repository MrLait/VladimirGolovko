import React from "react";
import {useTranslation} from "react-i18next";
import {Link} from "react-router-dom";
import style from  "./EventManagerArea.module.css"

const EventManagerArea = (props) => {
    const {t} = useTranslation();
    return (
        <div>
            <span>{props.isDeleteError ? t('Unable to delete because seats have already been purchased for the event.') : ""} </span>
            <table>
                <tbody>
                <tr>
                    <th>{t('Picture')}</th>
                    <th>{t('EventName')}</th>
                    <th>{t('Description')}</th>
                    <th>{t('EventStartsAt')}</th>
                    <th>{t('EventEndsAt')}</th>
                    <th>{t('AvailableOfSeats')}</th>
                    <th>{t('PricesStartFrom')}</th>
                    <th>{t('PricesTo')}</th>
                </tr>
                {props.events.map(
                    (e, arrayIndex) =>
                        <tr key={e.id}>
                            <td>
                                <div><img className={style.img} src={`${window.location.origin}/${e.imageUrl}`}/></div>
                            </td>
                            <td>{e.name}</td>
                            <td>{e.description}</td>
                            <td>{e.startDateTime}</td>
                            <td>{e.endDateTime}</td>
                            <td>{e.availableSeats}</td>
                            <td>{e.priceFrom}</td>
                            <td>{e.priceTo}</td>
                            <td>
                                <button onClick={() => {props.deleteEvent(e.id, arrayIndex);}}>{t('Delete')} </button>
                                <Link to={"update/event/" + e.id}>
                                    <button>{t('Update')} </button>
                                </Link>
                            </td>
                        </tr>
                )}
                </tbody>
            </table>
            <div>
                <Link to={"create/event"}>
                    <button>{t('Create event')} </button>
                </Link>
            </div>
        </div>
    )
}

export default EventManagerArea;
