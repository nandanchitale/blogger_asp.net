--
-- PostgreSQL database dump
--

-- Dumped from database version 12.18 (Debian 12.18-1.pgdg120+2)
-- Dumped by pg_dump version 16.2

-- Started on 2024-04-17 23:18:36

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 3012 (class 1262 OID 16384)
-- Name: blogger_db; Type: DATABASE; Schema: -; Owner: -
--

CREATE DATABASE blogger_db WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


\connect blogger_db

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 8 (class 2615 OID 16385)
-- Name: blogger; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA blogger;


--
-- TOC entry 208 (class 1255 OID 32769)
-- Name: getseq(); Type: FUNCTION; Schema: blogger; Owner: -
--

CREATE FUNCTION blogger.getseq() RETURNS bigint
    LANGUAGE plpgsql
    AS $$
	BEGIN
		return nextval('blogger.blogger_seq');
	END;
$$;


--
-- TOC entry 204 (class 1259 OID 16388)
-- Name: blogger_seq; Type: SEQUENCE; Schema: blogger; Owner: -
--

CREATE SEQUENCE blogger.blogger_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 207 (class 1259 OID 16413)
-- Name: post_comments; Type: TABLE; Schema: blogger; Owner: -
--

CREATE TABLE blogger.post_comments (
    id bigint DEFAULT nextval('blogger.blogger_seq'::regclass) NOT NULL,
    post_id bigint NOT NULL,
    user_id bigint NOT NULL,
    comment_text text NOT NULL,
    status character varying(20) NOT NULL,
    status_change_date timestamp without time zone NOT NULL
);


--
-- TOC entry 206 (class 1259 OID 16399)
-- Name: posts; Type: TABLE; Schema: blogger; Owner: -
--

CREATE TABLE blogger.posts (
    id bigint DEFAULT nextval('blogger.blogger_seq'::regclass) NOT NULL,
    title character varying(255) NOT NULL,
    author_id bigint NOT NULL,
    post_content text NOT NULL,
    status character varying(20) NOT NULL,
    status_change_date timestamp without time zone NOT NULL
);


--
-- TOC entry 205 (class 1259 OID 16390)
-- Name: users; Type: TABLE; Schema: blogger; Owner: -
--

CREATE TABLE blogger.users (
    id bigint DEFAULT nextval('blogger.blogger_seq'::regclass) NOT NULL,
    first_name character varying(150) NOT NULL,
    last_name character varying(150),
    username character varying(500),
    email character varying(500),
    password character varying(500),
    status character varying(20) NOT NULL,
    status_change_date timestamp without time zone NOT NULL
);


--
-- TOC entry 3006 (class 0 OID 16413)
-- Dependencies: 207
-- Data for Name: post_comments; Type: TABLE DATA; Schema: blogger; Owner: -
--

INSERT INTO blogger.post_comments VALUES (22, 13, 1, 'Amazing', 'ACTV', '2024-04-17 15:59:25.205437');
INSERT INTO blogger.post_comments VALUES (24, 13, 23, 'Nice post', 'ACTV', '2024-04-17 16:04:50.723529');
INSERT INTO blogger.post_comments VALUES (26, 13, 25, 'Nice Blog', 'ACTV', '2024-04-17 16:15:14.98513');


--
-- TOC entry 3005 (class 0 OID 16399)
-- Dependencies: 206
-- Data for Name: posts; Type: TABLE DATA; Schema: blogger; Owner: -
--

INSERT INTO blogger.posts VALUES (11, 'Test', 1, '<p>1234</p>', 'INAC', '2024-04-16 11:24:20.886822');
INSERT INTO blogger.posts VALUES (14, 'The Final Frontier', 1, '<p>There can be no thought of finishing for ‘aiming for the stars.’ Both figuratively and literally, it is a task to occupy the generations. And no matter how much progress one makes, there is always the thrill of just beginning.</p><p><br></p><p>There can be no thought of finishing for ‘aiming for the stars.’ Both figuratively and literally, it is a task to occupy the generations. And no matter how much progress one makes, there is always the thrill of just beginning.</p><p><br></p><blockquote><em>The dreams of yesterday are the hopes of today and the reality of tomorrow. Science has not yet mastered prophecy. We predict too much for the next year and yet far too little for the next ten.</em></blockquote><p><br></p><p>Spaceflights cannot be stopped. This is not the work of any one man or even a group of men. It is a historical process which mankind is carrying out in accordance with the natural laws of human development.</p>', 'ACTV', '2024-04-16 15:22:52.040578');
INSERT INTO blogger.posts VALUES (12, 'Man must explore, and this is exploration at its greatest', 1, '<p>Never in all their history have men been able truly to conceive of the world as one: a single sphere, a globe, having the qualities of a globe, a round earth in which all the directions eventually meet, in which there is no center because every point, or none, is center — an equal earth which all men occupy as equals. The airman''s earth, if free men make it, will be truly round: a globe in practice, not in theory.</p><p>Science cuts two ways, of course; its products can be used for both good and evil. But there''s no turning back from science. The early warnings about technological dangers also come from science.</p><p>What was most significant about the lunar voyage was not that man set foot on the Moon but that they set eye on the earth.</p><p>A Chinese tale tells of some men sent to harm a young girl who, upon seeing her beauty, become her protectors rather than her violators. That''s how I felt seeing the Earth for the first time. I could not help but love and cherish her.</p><p>For those who have seen the Earth from space, and for the hundreds and perhaps thousands more who will, the experience most certainly changes your perspective. The things that we share in our world are far more valuable than those which divide us.</p><h2><strong>The Final Frontier</strong></h2><p>There can be no thought of finishing for ‘aiming for the stars.’ Both figuratively and literally, it is a task to occupy the generations. And no matter how much progress one makes, there is always the thrill of just beginning.</p><p>There can be no thought of finishing for ‘aiming for the stars.’ Both figuratively and literally, it is a task to occupy the generations. And no matter how much progress one makes, there is always the thrill of just beginning.</p><blockquote><em>The dreams of yesterday are the hopes of today and the reality of tomorrow. Science has not yet mastered prophecy. We predict too much for the next year and yet far too little for the next ten.</em></blockquote><p>Spaceflights cannot be stopped. This is not the work of any one man or even a group of men. It is a historical process which mankind is carrying out in accordance with the natural laws of human development.</p><h2><strong>Reaching for the Stars</strong></h2><p>As we got further and further away, it [the Earth] diminished in size. Finally it shrank to the size of a marble, the most beautiful you can imagine. That beautiful, warm, living object looked so fragile, so delicate, that if you touched it with a finger it would crumble and fall apart. Seeing this has to change a man.</p><p><a href="https://startbootstrap.github.io/startbootstrap-clean-blog/post.html#!" target="_blank" style="background-color: rgb(255, 255, 255); color: var(--bs-link-color); --darkreader-inline-bgcolor: #181a1b; --darkreader-inline-color: var(--darkreader-text--bs-link-color);" data-darkreader-inline-bgcolor="" data-darkreader-inline-color=""><img src="https://startbootstrap.github.io/startbootstrap-clean-blog/assets/img/post-sample-image.jpg" alt="..."></a><em style="background-color: rgb(255, 255, 255); color: rgb(108, 117, 125); --darkreader-inline-bgcolor: #181a1b; --darkreader-inline-color: #9e9689;" data-darkreader-inline-bgcolor="" data-darkreader-inline-color="">To go places and do things that have never been done before – that’s what living is all about.</em></p><p><br></p><p>Space, the final frontier. These are the voyages of the Starship Enterprise. Its five-year mission: to explore strange new worlds, to seek out new life and new civilizations, to boldly go where no man has gone before.</p><p>As I stand out here in the wonders of the unknown at Hadley, I sort of realize there’s a fundamental truth to our nature, Man must explore, and this is exploration at its greatest.</p><p>Placeholder text by&nbsp;<a href="http://spaceipsum.com/" target="_blank" style="color: var(--bs-link-color); --darkreader-inline-color: var(--darkreader-text--bs-link-color);" data-darkreader-inline-color="">Space Ipsum</a>&nbsp;· Images by&nbsp;<a href="https://www.flickr.com/photos/nasacommons/" target="_blank" style="color: var(--bs-link-color); --darkreader-inline-color: var(--darkreader-text--bs-link-color);" data-darkreader-inline-color="">NASA on The Commons</a></p>', 'INAC', '2024-04-16 15:43:57.31553');
INSERT INTO blogger.posts VALUES (13, 'Man must explore, and this is exploration at its greatest !', 1, '<p>Never in all their history have men been able truly to conceive of the world as one: a single sphere, a globe, having the qualities of a globe, a round earth in which all the directions eventually meet, in which there is no center because every point, or none, is center — an equal earth which all men occupy as equals. The airman''s earth, if free men make it, will be truly round: a globe in practice, not in theory.</p><p>Science cuts two ways, of course; its products can be used for both good and evil. But there''s no turning back from science. The early warnings about technological dangers also come from science.</p><p><br></p><p>What was most significant about the lunar voyage was not that man set foot on the Moon but that they set eye on the earth.</p><p><br></p><p>A Chinese tale tells of some men sent to harm a young girl who, upon seeing her beauty, become her protectors rather than her violators. That''s how I felt seeing the Earth for the first time. I could not help but love and cherish her.</p><p><br></p><p>For those who have seen the Earth from space, and for the hundreds and perhaps thousands more who will, the experience most certainly changes your perspective. The things that we share in our world are far more valuable than those which divide us.</p><p><br></p><p>Thanks</p>', 'ACTV', '2024-04-16 16:15:04.998887');
INSERT INTO blogger.posts VALUES (15, 'Reaching for the Stars', 1, '<p>As we got further and further away, it [the Earth] diminished in size. Finally it shrank to the size of a marble, the most beautiful you can imagine. That beautiful, warm, living object looked so fragile, so delicate, that if you touched it with a finger it would crumble and fall apart. Seeing this has to change a man.</p><p><a href="https://startbootstrap.github.io/startbootstrap-clean-blog/post.html#!" target="_blank" style="color: var(--bs-link-color);"><img src="https://startbootstrap.github.io/startbootstrap-clean-blog/assets/img/post-sample-image.jpg" alt="..."></a><em>To go places and do things that have never been done before – that’s what living is all about.</em></p><p><br></p><p>Space, the final frontier. These are the voyages of the Starship Enterprise. Its five-year mission: to explore strange new worlds, to seek out new life and new civilizations, to boldly go where no man has gone before.</p><p>As I stand out here in the wonders of the unknown at Hadley, I sort of realize there’s a fundamental truth to our nature, Man must explore, and this is exploration at its greatest.</p>', 'ACTV', '2024-04-16 16:17:04.852513');
INSERT INTO blogger.posts VALUES (16, 'Test', 1, '<p>1234</p>', 'INAC', '2024-04-16 16:19:12.197548');
INSERT INTO blogger.posts VALUES (17, 'It is a long established fact a reader be distracted', 1, '<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Reiciendis, eius mollitia suscipit, quisquam doloremque distinctio perferendis et doloribus unde architecto optio laboriosam porro adipisci sapiente officiis nemo accusamus ad praesentium? Esse minima nisi et. Dolore perferendis, enim praesentium omnis, iste doloremque quia officia optio deserunt molestiae voluptates soluta architecto tempora.''</p><p><br></p><p>Molestiae cupiditate inventore animi, maxime sapiente optio, illo est nemo veritatis repellat sunt doloribus nesciunt! Minima laborum magni reiciendis qui voluptate quisquam voluptatem soluta illo eum ullam incidunt rem assumenda eveniet eaque sequi deleniti tenetur dolore amet fugit perspiciatis ipsa, odit. Nesciunt dolor minima esse vero ut ea, repudiandae suscipit!</p>', 'INAC', '2024-04-16 16:52:54.866946');
INSERT INTO blogger.posts VALUES (19, 'It is a long established fact a reader be distracted', 1, '<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Reiciendis, eius mollitia suscipit, quisquam doloremque distinctio perferendis et doloribus unde architecto optio laboriosam porro adipisci sapiente officiis nemo accusamus ad praesentium? Esse minima nisi et. Dolore perferendis, enim praesentium omnis, iste doloremque quia officia optio deserunt molestiae voluptates soluta architecto tempora.</p><p>Molestiae cupiditate inventore animi, maxime sapiente optio, illo est nemo veritatis repellat sunt doloribus nesciunt! Minima laborum magni reiciendis qui voluptate quisquam voluptatem soluta illo eum ullam incidunt rem assumenda eveniet eaque sequi deleniti tenetur dolore amet fugit perspiciatis ipsa, odit. Nesciunt dolor minima esse vero ut ea, repudiandae suscipit!</p>', 'INAC', '2024-04-17 16:16:12.944281');
INSERT INTO blogger.posts VALUES (27, 'Test', 1, '<p><strong>Test blog post</strong></p><h1>Hello Theredfgsdfg</h1>', 'INAC', '2024-04-17 23:16:21.860537');


--
-- TOC entry 3004 (class 0 OID 16390)
-- Dependencies: 205
-- Data for Name: users; Type: TABLE DATA; Schema: blogger; Owner: -
--

INSERT INTO blogger.users VALUES (1, 'nandan', 'chitale', 'nandanchitale', 'nandanchitale@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 'ACTV', '2024-04-15 17:08:18.482292');
INSERT INTO blogger.users VALUES (20, 'Nandan', 'Chitale', 'Nandan.Chitale', 'nandanchitale@gmail.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'ACTV', '2024-04-17 12:54:09.081767');
INSERT INTO blogger.users VALUES (21, 'Nandan', 'Chitale', 'Nandan.Chitale', 'nandanchitale99@gmail.com', 'c92e8ac33e6dc1c9810fa79f383635b065e64cddaafc10a8e180c6e7be6debf4', 'ACTV', '2024-04-17 12:54:33.197463');
INSERT INTO blogger.users VALUES (23, 'Atharva', 'Chitnavis', 'Atharva.Chitnavis', 'atharvachitnavis@gmail.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'ACTV', '2024-04-17 16:04:13.182197');
INSERT INTO blogger.users VALUES (25, 'John', 'Matten', 'John.Matten', 'john.doe@domain.com', '8bb0cf6eb9b17d0f7d22b456f121257dc1254e1f01665370476383ea776df414', 'ACTV', '2024-04-17 16:14:54.771095');


--
-- TOC entry 3013 (class 0 OID 0)
-- Dependencies: 204
-- Name: blogger_seq; Type: SEQUENCE SET; Schema: blogger; Owner: -
--

SELECT pg_catalog.setval('blogger.blogger_seq', 27, true);


--
-- TOC entry 2873 (class 2606 OID 16421)
-- Name: post_comments post_comments_pk; Type: CONSTRAINT; Schema: blogger; Owner: -
--

ALTER TABLE ONLY blogger.post_comments
    ADD CONSTRAINT post_comments_pk PRIMARY KEY (id);


--
-- TOC entry 2871 (class 2606 OID 16407)
-- Name: posts posts_pk; Type: CONSTRAINT; Schema: blogger; Owner: -
--

ALTER TABLE ONLY blogger.posts
    ADD CONSTRAINT posts_pk PRIMARY KEY (id);


--
-- TOC entry 2869 (class 2606 OID 16398)
-- Name: users users_pk; Type: CONSTRAINT; Schema: blogger; Owner: -
--

ALTER TABLE ONLY blogger.users
    ADD CONSTRAINT users_pk PRIMARY KEY (id);


--
-- TOC entry 2875 (class 2606 OID 16427)
-- Name: post_comments comment_user_fk; Type: FK CONSTRAINT; Schema: blogger; Owner: -
--

ALTER TABLE ONLY blogger.post_comments
    ADD CONSTRAINT comment_user_fk FOREIGN KEY (user_id) REFERENCES blogger.users(id);


--
-- TOC entry 2874 (class 2606 OID 16408)
-- Name: posts post_auther_fk; Type: FK CONSTRAINT; Schema: blogger; Owner: -
--

ALTER TABLE ONLY blogger.posts
    ADD CONSTRAINT post_auther_fk FOREIGN KEY (author_id) REFERENCES blogger.users(id);


--
-- TOC entry 2876 (class 2606 OID 16422)
-- Name: post_comments post_fk; Type: FK CONSTRAINT; Schema: blogger; Owner: -
--

ALTER TABLE ONLY blogger.post_comments
    ADD CONSTRAINT post_fk FOREIGN KEY (post_id) REFERENCES blogger.posts(id);


-- Completed on 2024-04-17 23:18:37

--
-- PostgreSQL database dump complete
--

