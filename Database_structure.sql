create table blogger.users(
	id int8 not null default nextval('blogger.blogger_seq'),
	first_name varchar(150) not null,
	last_name varchar(150),
	username varchar(500),
	email varchar(500),
	"password" varchar(500),
	status varchar(20) not null,
	status_change_date timestamp not null,
	
	constraint users_pk primary key (id)
);

CREATE TABLE blogger.posts (
    id int8 not null default nextval('blogger.blogger_seq'),
    title VARCHAR(255) NOT NULL,
    author_id int8 not null,
    post_content TEXT NOT NULL,
    status varchar(20) not null,
	status_change_date timestamp not null,
	
	constraint posts_pk primary key (id),
	constraint post_auther_fk foreign key (author_id) references blogger.users(id)
);

CREATE TABLE blogger.post_comments (
    id int8 not null default nextval('blogger.blogger_seq'),
    user_id int8 not null,
	status varchar(20) not null,
	status_change_date timestamp not null,
);

CREATE TABLE post_comments (
	
    post_id int8
    PRIMARY KEY (post_id, comment_id)
);
