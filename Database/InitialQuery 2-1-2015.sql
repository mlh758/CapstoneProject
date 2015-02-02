CREATE DATABASE ONCALL_ASSISTANT;

use ONCALL_ASSISTANT;

CREATE TABLE OUT_OF_OFFICE_REASON
(
	reasonID			int primary key identity,
	reason				varchar(60)		NOT NULL
);

CREATE TABLE OUT_OF_OFFICE
(
	outOfOfficeID		int primary key identity,
	numHours			int				NOT NULL,
	_date				date			NOT NULL,
	reasonID			int				NOT NULL,

	CONSTRAINT reason_FK FOREIGN KEY (reasonID) REFERENCES OUT_OF_OFFICE_REASON (reasonID),
	CONSTRAINT chkHours CHECK (numHours > 0)
);

CREATE TABLE _APPLICATION
(
	applicationID		 int primary key identity,
	name				 varchar(10)	 NOT NULL,
	appPriority			 int			 NOT NULL
);


CREATE TABLE EMPLOYEE
(
	employeeID			 int primary key,
	firstName			 varchar(30)	 NOT NULL,
	lastName			 varchar(30)	 NOT NULL,
	alottedVacationHours int			 NOT NULL,
	_role				 varchar (30)	 NOT NULL,
	applicationID		 int			 NOT NULL,
	
	CONSTRAINT application_FK FOREIGN KEY (applicationID) REFERENCES _APPLICATION (applicationID),
	CONSTRAINT chkVacationHours CHECK (alottedVacationHours BETWEEN 0 AND 300)
		 
);

CREATE TABLE ONCALL_ROTATION
(
	rotationID			int primary key identity,
	startDate			date			NOT NULL,
	endDate				date			NOT NULL,
	isPrimary			bit				NOT NULL, --  sql server doesn't support a boolean type.  
												  --  A bit takes 0, 1, or null 												 
	employeeID			int				NOT NULL,

	CONSTRAINT employee_FK FOREIGN KEY (employeeID) REFERENCES EMPLOYEE (employeeID),
	CONSTRAINT chkDateRange CHECK (endDate >= startDate),
	CONSTRAINT chkRotationStartDate CHECK (DATENAME(weekday, startDate) = 'Wednesday'),
	CONSTRAINT chkRotationEndDate CHECK (DATENAME(weekday, endDate) = 'Tuesday')
);

CREATE TABLE PAID_HOLIDAY
(
	holidayID			int primary key identity,
	name				varchar(60)		NOT NULL
);

CREATE TABLE HAS_PAID_HOLIDAY
(
	holidayID			int			    NOT NULL,
	rotationID			int			    NOT NULL,
	holidayDate			date		    NOT NULL,

	CONSTRAINT holiday_FK FOREIGN KEY (holidayID) REFERENCES PAID_HOLIDAY (holidayID),
	CONSTRAINT roration_FK FOREIGN KEY (rotationID) REFERENCES ONCALL_ROTATION (rotationID)
);


--------------Seed Data-----------------
INSERT INTO PAID_HOLIDAY VALUES ('New Year''s Day');
INSERT INTO PAID_HOLIDAY VALUES ('Martin Luther King Day');
INSERT INTO PAID_HOLIDAY VALUES ('President''s Day');
INSERT INTO PAID_HOLIDAY VALUES ('Memorial Day');
INSERT INTO PAID_HOLIDAY VALUES ('Independence Day');
INSERT INTO PAID_HOLIDAY VALUES ('Labor Day');
INSERT INTO PAID_HOLIDAY VALUES ('Columbus Day');
INSERT INTO PAID_HOLIDAY VALUES ('Veterans Day');
INSERT INTO PAID_HOLIDAY VALUES ('Thanksgiving');
INSERT INTO PAID_HOLIDAY VALUES ('Christmas');

INSERT INTO OUT_OF_OFFICE_REASON VALUES('Personal Day');
INSERT INTO OUT_OF_OFFICE_REASON VALUES('Sick Day');
INSERT INTO OUT_OF_OFFICE_REASON VALUES('Floating Holiday');
INSERT INTO OUT_OF_OFFICE_REASON VALUES('Vacation');
INSERT INTO OUT_OF_OFFICE_REASON VALUES('Bereavement');
INSERT INTO OUT_OF_OFFICE_REASON VALUES('Unavailable for On-Call');
INSERT INTO OUT_OF_OFFICE_REASON VALUES('Military Duty');
INSERT INTO OUT_OF_OFFICE_REASON VALUES('Training/Technical Conference');
INSERT INTO OUT_OF_OFFICE_REASON VALUES('Jury Duty');
INSERT INTO OUT_OF_OFFICE_REASON VALUES('Other');


/*
*	General Comments:	
*		+ All statements written in Transact-SQL
*		+ For most tables I added an identity constraint for the primary key fields.
*         This will auto-generate the ID for that table.  
*		+ This query just sets up the tables and seeds tables OUT_OF_OFFICE_REASON and PAID_HOLIDAY.
*		   Be thinking about Stored Procedures we'll want to add.  OR do we want to use Linq 
*		   instead?
*
*	OUT_OF_OFFICE Table:
*		+ Has a startDate attribute but does NOT have an endDate attribute
*		+ To determine the end date, (assuming numHours > 8): 
*		  divide by 8 to determine the number of days, add that result -1 to startDate.
*		+ Ex. startDate = 2/1/2015, numHours = 16  
*		  the ending date = 2/1/2015 + (16/8 -1) = 2/2/2015	 
* 
*	EMPLOYEE Table:
*		+ Is there other employee information we need to track? Email? Phone? Address? etc.
*		+ Not sure what the 'role' attribute refers to
*		+ Do we want to autogenerate an employee's ID, or will Paul want to enter an existing ID?
*         For now I didn't not give it an identity constraint.
*
*	_APPLICAION table:
*		+ For the 'appPriority' attribute, what is the range?  I can add a constraint to check
*         that values fall within that range.
*		
*/