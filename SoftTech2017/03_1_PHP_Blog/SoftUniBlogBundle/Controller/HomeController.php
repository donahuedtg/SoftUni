<?php

namespace SoftUniBlogBundle\Controller;

use Doctrine\ORM\Mapping\Cache;
use Sensio\Bundle\FrameworkExtraBundle\Configuration\Method;
use Sensio\Bundle\FrameworkExtraBundle\Configuration\Route;
use Sensio\Bundle\FrameworkExtraBundle\Configuration\Security;
use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use SoftUniBlogBundle\Entity\Article;


class HomeController extends Controller
{
    /**
     * @Route("/", name="blog_index")
     * @Method("GET")
     */
    public function indexAction()
    {
		$articles = $this->getDoctrine()->getRepository(Article::class)->findAll();
		
        return $this->render('blog/index.html.twig',
			['articles' => $articles]);
    }
}
