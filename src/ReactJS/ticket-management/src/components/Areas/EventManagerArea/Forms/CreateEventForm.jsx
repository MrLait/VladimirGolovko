import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {required} from "../../../Utils/validators";
import {Input} from "../../../../common/FormsControls/FormsControls";
import {Redirect} from "react-router";
import renderDatePicker from "../../../../common/FormsControls/renderDatePicker";

const CreateEventForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <div>
                <Field placeholder={t("Name")} name={'name'} validate={[required]} component={Input}/>
            </div>
            <div>
                <Field placeholder={t("Description")} name={"description"} validate={[required]} component={Input}/>
            </div>
            <div>
                <Field type={'number'} placeholder={t("LayoutId")} name={"layoutId"} validate={[required]}
                       component={Input}/>
            </div>
            <div>
                <Field placeholder={t("StartDateTime")} name={"startDateTime"} validate={[required]}
                       component={renderDatePicker}/>
            </div>
            <div>
                <Field placeholder={t("EndDateTime")} name={"endDateTime"} validate={[required]}
                       component={renderDatePicker}/>
            </div>
            <div>
                <Field placeholder={t("ImageUrl")} name={"imageUrl"} validate={[required]} component={Input}/>
            </div>

            <div>
                {
                    props.event && props.event.eventAreas.length > 0
                        ? props.event.eventAreas.map(ea =>
                            <div>
                                <Field type={'number'} placeholder={t("Price")} name={`eventAreas[${ea.id}].price`}
                                       validate={[required]}
                                       component={Input}/>
                            </div>
                        )
                        : ""
                }
            </div>
            <div>
                {props.event && props.event.eventAreas
                    ?
                    <button disabled={props.inProgress}>
                        {t('PublishEvent')}
                    </button>
                    :
                    <button disabled={props.inProgress}>
                        {t('CreateEvent')}
                    </button>
                }
                {props.isCreateEventSuccessful
                    ? <Redirect to={'/eventManagerArea'}/>
                    : ""}
            </div>
        </form>
    )
}

export default reduxForm(
    {
        form: 'createEvent'
    }
)(CreateEventForm)
